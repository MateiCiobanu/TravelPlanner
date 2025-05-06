using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Application.Services;

namespace TravelPlanner.WebUI.Controllers
{   
    [Route("itinerary")] 
    public class ItineraryViewController : Controller
    {
        private readonly ItinerarySuggestionService _suggestionService;
        private readonly ItineraryViewModelMapper _mapper;
        
        public ItineraryViewController(
            ItinerarySuggestionService suggestionService,
            ItineraryViewModelMapper mapper)
        {
            _suggestionService = suggestionService;
            _mapper = mapper;
        }


        [HttpGet("{destination}")]         
        public async Task<IActionResult> Display(string destination, DateTime startDate, DateTime endDate)
        {
            try
            {
                var request = new PlaceSuggestionRequestDto
                {
                    Destination = destination,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var suggestions = await _suggestionService.GenerateSuggestionsAsync(request);
                var viewModel = _mapper.MapToViewModel(destination, suggestions);
                
                return View("~/Views/ItineraryView/Display.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to generate itinerary: {ex.Message}");
            }
        }
        [HttpGet("/test")]
            public IActionResult Test()
            {
                return Content("MVC is working!");
            }
    }
}
