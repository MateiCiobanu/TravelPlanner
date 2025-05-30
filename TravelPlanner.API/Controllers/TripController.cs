using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;
using Newtonsoft.Json;

namespace TravelPlanner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepo;
        private readonly IItineraryItemRepository _itineraryRepo;

        private readonly IUserRepository _userRepo;

        public TripController(ITripRepository tripRepo, IItineraryItemRepository itineraryRepo, IUserRepository userRepo)
        {
            _tripRepo = tripRepo;
            _itineraryRepo = itineraryRepo;
            _userRepo = userRepo;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveTrip([FromBody] SaveTripRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest("Model validation failed: " + errors);
            }

            var existingTrip = await _tripRepo.GetTripByUserDestinationAndDatesAsync(
                request.UserId,
                request.Destination,
                request.StartDate,
                request.EndDate
            );

            if (existingTrip != null)
            {
                return Conflict("Trip already exists for this user with the same destination and dates.");
            }

 
            var newTrip = new Trip
            {
                UserId = request.UserId,
                Title = request.Title ?? $"Trip to {request.Destination}",
                Destination = request.Destination,
                StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc),
                Status = "Saved"
            };

            var createdTrip = await _tripRepo.CreateAsync(newTrip);

            var items = request.ItineraryItems.Select(item => new ItineraryItem
            {
                TripId = createdTrip.Id,
                GooglePlaceId = item.GooglePlaceId,
                PlaceName = item.PlaceName,
                PlaceCategory = item.PlaceCategory,
                DayNumber = item.DayNumber,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Duration = item.Duration ?? 0
            });

            await _itineraryRepo.AddRangeAsync(items);

         
            foreach (var email in request.FriendEmails.Distinct())
            {
                var friend = await _userRepo.GetByEmailAsync(email);
                if (friend == null) continue;

                var friendExistingTrip = await _tripRepo.GetTripByUserDestinationAndDatesAsync(
                    friend.Id,
                    request.Destination,
                    request.StartDate,
                    request.EndDate
                );

                if (friendExistingTrip != null) continue; 

                var friendTrip = new Trip
                {
                    UserId = friend.Id,
                    Title = request.Title ?? $"Trip to {request.Destination}",
                    Destination = request.Destination,
                    StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc),
                    Status = "Saved"
                };

                var savedTrip = await _tripRepo.CreateAsync(friendTrip);

                var clonedItems = request.ItineraryItems.Select(item => new ItineraryItem
                {
                    TripId = savedTrip.Id,
                    GooglePlaceId = item.GooglePlaceId,
                    PlaceName = item.PlaceName,
                    PlaceCategory = item.PlaceCategory,
                    DayNumber = item.DayNumber,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Duration = item.Duration ?? 0
                });

                await _itineraryRepo.AddRangeAsync(clonedItems);
            }

            return Ok(new { success = true, tripId = createdTrip.Id });
        }
    }
}