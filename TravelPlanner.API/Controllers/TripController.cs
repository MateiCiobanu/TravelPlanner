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
        private readonly ITripRepository _tripRepository;
        private readonly IItineraryItemRepository _itineraryItemRepository;
        private readonly IMapper _mapper;

        public TripController(ITripRepository tripRepository, IItineraryItemRepository itineraryItemRepository, IMapper mapper)
        {
            _tripRepository = tripRepository;
            _itineraryItemRepository = itineraryItemRepository;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<TripDto>>> GetUserTrips(int userId)
        {
            try
            {
                var trips = await _tripRepository.GetByUserIdAsync(userId);
                var tripDtos = new List<TripDto>();

                foreach (var trip in trips)
                {
                    var itineraryItems = await _itineraryItemRepository.GetByTripIdAsync(trip.Id);
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
                
                var createdTrip = await _tripRepository.CreateAsync(trip);

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
                
                var updatedTrip = await _tripRepository.UpdateAsync(trip);
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
                var success = await _tripRepository.DeleteAsync(id);
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
                
                var createdItems = await _itineraryItemRepository.AddRangeAsync(items);
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
                
                var createdItem = await _itineraryItemRepository.CreateAsync(item);
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
                
                var updatedItem = await _itineraryItemRepository.UpdateAsync(item);
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
                var success = await _itineraryItemRepository.DeleteAsync(itemId);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}