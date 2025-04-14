using TravelPlanner.Application.ViewModels;
using TravelPlanner.Application.DTOs;

namespace TravelPlanner.Application.Services
{
    public class ItineraryViewModelMapper
    {
        public ItineraryViewModel MapToViewModel(string destination, PlaceSuggestionsResponseDto suggestionsDto)
        {
            var viewModel = new ItineraryViewModel
            {
                Destination = destination,
                Days = new List<DayViewModel>()
            };

            foreach (var dailySuggestion in suggestionsDto.DailySuggestions)
            {
                var dayViewModel = new DayViewModel
                {
                    Date = dailySuggestion.Date,
                    Events = new List<EventViewModel>()
                };

                var startHour = 9; 
                
                foreach (var place in dailySuggestion.Places)
                {
                    var eventDuration = GetEventDuration(place.Category);
                    var startTime = new TimeSpan(startHour, 0, 0);
                    var endTime = startTime.Add(TimeSpan.FromHours(eventDuration));
                    
                    dayViewModel.Events.Add(new EventViewModel
                    {
                        StartTime = startTime,
                        EndTime = endTime,
                        Description = place.Name,
                        PlaceCategory = place.Category,
                        Address = place.Address,
                        Rating = place.Rating
                    });
                    
                    startHour += eventDuration;
                }
                
                viewModel.Days.Add(dayViewModel);
            }
            
            return viewModel;
        }
        
        // Helper method to determine duration based on place category
        private int GetEventDuration(string category)
        {
            return category.ToLower() switch
            {
                "restaurant" => 2,
                "cafe" => 1,
                "museum" => 3,
                "tourist_attraction" => 2,
                "park" => 2,
                "shopping_mall" => 3,
                _ => 1
            };
        }
    }
}