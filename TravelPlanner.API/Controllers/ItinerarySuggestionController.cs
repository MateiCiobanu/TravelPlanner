using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Application.Services;

namespace TravelPlanner.API.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class ItinerarySuggestionsController : ControllerBase
    {
        private readonly ItinerarySuggestionService _suggestionService;

        public ItinerarySuggestionsController(ItinerarySuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
        }

        [HttpPost("suggest")]
        public async Task<ActionResult<PlaceSuggestionsResponseDto>> GetSuggestions([FromBody] PlaceSuggestionRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var suggestions = await _suggestionService.GenerateSuggestionsAsync(request);
                return Ok(suggestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to generate suggestions: {ex.Message}");
            }
        }
    }
}