using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services;
using TravelPlanner.Application.DTOs;

namespace TravelPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GooglePlacesController : ControllerBase
    {
        private readonly GooglePlacesService _googlePlacesService;

        public GooglePlacesController(GooglePlacesService googlePlacesService)
        {
            _googlePlacesService = googlePlacesService;
        }

        [HttpGet("places")]
        public async Task<ActionResult<List<PlaceDto>>> GetPlaces([FromQuery] string category, [FromQuery] string location)
        {
            if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(location))
                return BadRequest("Both 'category' and 'location' query parameters are required.");

            try
            {
                var places = await _googlePlacesService.GetPlacesAsync(category, location);

                if (places.Count == 0)
                    return NotFound($"No {category} found in {location} ");

                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong: {ex.Message}");
            }
        }


        [HttpGet("details")]
        public async Task<ActionResult<PlaceDto>> GetPlaceDetailedInfo([FromQuery] string place_id)
        {
            if (string.IsNullOrWhiteSpace(place_id))
                return BadRequest("The 'place_id' query parameter is required.");

            try
            {
                var detailedInformation = await _googlePlacesService.GetPlaceDetailedInfoAsync(place_id);

                if (detailedInformation == null)
                    return NotFound($"There are is no detailed information about this place, {place_id}");

                return Ok(detailedInformation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong: {ex.Message}");
            }
        }
    }
}
