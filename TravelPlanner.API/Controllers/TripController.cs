using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Geçici olarak kaldırıldı
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepo;
        private readonly IItineraryItemRepository _itineraryRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        public TripController(ITripRepository tripRepo, IItineraryItemRepository itineraryRepo, IMapper mapper, IUserRepository userRepo)
        {
            _tripRepo = tripRepo;
            _itineraryRepo = itineraryRepo;
            _mapper = mapper;
            _userRepo = userRepo;

        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<TripDto>>> GetUserTrips(int userId)
        {
            try
            {
                var trips = await _tripRepo.GetByUserIdAsync(userId);
                var tripDtos = new List<TripDto>();

                foreach (var trip in trips)
                {
                    var itineraryItems = await _itineraryRepo.GetByTripIdAsync(trip.Id);
                    var tripDto = _mapper.Map<TripDto>(trip);
                    tripDto.ItineraryItems = _mapper.Map<List<ItineraryItemDto>>(itineraryItems);
                    tripDtos.Add(tripDto);
                }

                return Ok(tripDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<TripDto>> CreateTrip([FromBody] TripCreationDto tripDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Manual mapping to ensure all required fields are set
                var trip = new Trip
                {
                    UserId = 1, // Şimdilik sabit user ID
                    Title = tripDto.Title ?? $"Trip to {tripDto.Destination}",
                    Destination = tripDto.Destination,
                    StartDate = tripDto.StartDate,
                    EndDate = tripDto.EndDate,
                    Status = "planning"
                };

                Console.WriteLine($"Creating trip: UserId={trip.UserId}, Title={trip.Title}, Destination={trip.Destination}");

                var createdTrip = await _tripRepo.CreateAsync(trip);

                var result = new TripDto
                {
                    Id = createdTrip.Id,
                    UserId = createdTrip.UserId,
                    Title = createdTrip.Title,
                    Destination = createdTrip.Destination,
                    StartDate = createdTrip.StartDate,
                    EndDate = createdTrip.EndDate,
                    Status = createdTrip.Status,
                    ItineraryItems = new List<ItineraryItemDto>()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating trip: {ex}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error creating trip: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TripDto>> UpdateTrip(int id, [FromBody] TripDto tripDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var trip = _mapper.Map<Trip>(tripDto);
                trip.Id = id;

                var updatedTrip = await _tripRepo.UpdateAsync(trip);
                var result = _mapper.Map<TripDto>(updatedTrip);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            try
            {
                var success = await _tripRepo.DeleteAsync(id);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{tripId}/itinerary")]
        public async Task<ActionResult<List<ItineraryItemDto>>> AddItineraryItems(int tripId, [FromBody] List<ItineraryItemDto> itemDtos)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var items = _mapper.Map<List<ItineraryItem>>(itemDtos);
                foreach (var item in items)
                {
                    item.TripId = tripId;
                }

                var createdItems = await _itineraryRepo.AddRangeAsync(items);
                var result = _mapper.Map<List<ItineraryItemDto>>(createdItems);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{tripId}/itinerary/single")]
        public async Task<ActionResult<ItineraryItemDto>> AddItineraryItem(int tripId, [FromBody] ItineraryItemDto itemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var item = _mapper.Map<ItineraryItem>(itemDto);
                item.TripId = tripId;

                var createdItem = await _itineraryRepo.CreateAsync(item);
                var result = _mapper.Map<ItineraryItemDto>(createdItem);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("itinerary/{itemId}")]
        public async Task<ActionResult<ItineraryItemDto>> UpdateItineraryItem(int itemId, [FromBody] ItineraryItemDto itemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var item = _mapper.Map<ItineraryItem>(itemDto);
                item.Id = itemId;

                var updatedItem = await _itineraryRepo.UpdateAsync(item);
                var result = _mapper.Map<ItineraryItemDto>(updatedItem);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("itinerary/{itemId}")]
        public async Task<IActionResult> DeleteItineraryItem(int itemId)
        {
            try
            {
                var success = await _itineraryRepo.DeleteAsync(itemId);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
