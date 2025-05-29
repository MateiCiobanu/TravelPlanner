using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Application.Services;
using Microsoft.Extensions.Logging;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application.Services
{
    public class ItinerarySuggestionService 
    {
        private readonly GooglePlacesService _googlePlacesService;
        private readonly ILogger<ItinerarySuggestionService> _logger;
        private readonly Random _random = new Random();

        private readonly List<string> _categories = new List<string>
        {
            "attractions",       
            "museums",           
            "restaurants",       
            "parks",             
            "shopping",          
            "cafes"              
        };


        public ItinerarySuggestionService(
            GooglePlacesService googlePlacesService,
            ILogger<ItinerarySuggestionService> logger)
        {
            _googlePlacesService = googlePlacesService;
            _logger = logger;
        }

        public async Task<PlaceSuggestionsResponseDto> GenerateSuggestionsAsync(PlaceSuggestionRequestDto request)
        {
            _logger.LogInformation($"Generating itinerary suggestions for {request.Destination} from {request.StartDate:yyyy-MM-dd} to {request.EndDate:yyyy-MM-dd}");

            var response = new PlaceSuggestionsResponseDto();
            
            int totalDays = (int)(request.EndDate - request.StartDate).TotalDays + 1;
            _logger.LogInformation($"Trip duration: {totalDays} days");


            var preferredCategories = _categories;

            var travelerTypeName = request.TravelerType?.TravelerTypeName?.ToLowerInvariant() ?? "";

            switch (travelerTypeName)
            {
                case "culture explorer":
                    preferredCategories = new List<string> { "museums", "attractions", "cafes" };
                    break;
                case "chill seeker":
                    preferredCategories = new List<string> { "parks", "cafes", "shopping" };
                    break;
                case "night owl":
                    preferredCategories = new List<string> { "restaurants", "bars", "clubs" };
                    break;
                case "foodie adventurer":
                    preferredCategories = new List<string> { "restaurants", "cafes", "markets" };
                    break;
                case "urban explorer":
                    preferredCategories = new List<string> { "shopping", "museums", "attractions" };
                    break;
                case "nature enthusiast":
                    preferredCategories = new List<string> { "parks", "nature", "trails" };
                    break;
                default:
                    preferredCategories = _categories;
                    break;
            }

            var allPlacesByCategory = new Dictionary<string, List<PlaceSuggestion>>();
            
            foreach (var category in _categories)
            {
                try
                {
                    var searchQuery = $"{category} in {request.Destination}";
                    var places = await _googlePlacesService.GetPlacesAsync(searchQuery, request.Destination);
                    
                    if (places != null && places.Count > 0)
                    {
                        allPlacesByCategory[category] = places.Select(p => new PlaceSuggestion
                        {
                            GooglePlaceId = p.PlaceId,
                            Name = p.Name,
                            Category = category,
                            Address = p.Address,
                            Rating = p.Rating,
                            Latitude = p.Latitude,
                            Longitude = p.Longitude
                        }).ToList();
                        
                        _logger.LogInformation($"Found {allPlacesByCategory[category].Count} places for category '{category}'");
                    }
                    else
                    {
                        _logger.LogWarning($"No places found for category '{category}' in {request.Destination}");
                        allPlacesByCategory[category] = new List<PlaceSuggestion>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error fetching places for category '{category}'");
                    allPlacesByCategory[category] = new List<PlaceSuggestion>();
                }
            }

            for (int day = 1; day <= totalDays; day++)
            {
                var currentDate = request.StartDate.AddDays(day - 1);
                var dailySuggestion = new DailyPlaceSuggestions
                {
                    DayNumber = day,
                    Date = currentDate,
                    Places = new List<PlaceSuggestion>()
                };

                var morningActivities = GetRandomPlaces(
                    allPlacesByCategory.GetValueOrDefault("attractions", new List<PlaceSuggestion>())
                        .Concat(allPlacesByCategory.GetValueOrDefault("museums", new List<PlaceSuggestion>()))
                        .ToList(),
                    _random.Next(1, 3)
                );
                dailySuggestion.Places.AddRange(morningActivities);

                var lunchPlace = GetRandomPlaces(
                    allPlacesByCategory.GetValueOrDefault("restaurants", new List<PlaceSuggestion>())
                        .Concat(allPlacesByCategory.GetValueOrDefault("cafes", new List<PlaceSuggestion>()))
                        .ToList(), 
                    1);
                    _logger.LogInformation($"Found {lunchPlace.Count} lunch places for this day.");
                    if (lunchPlace.Any())
                    {
                        // Log the names of the places directly for better insight
                        _logger.LogInformation($"Lunch places: {string.Join(", ", lunchPlace.Select(lp => lp.Name))}");
                    }
                    else
                    {
                        _logger.LogWarning("No lunch places found.");
                    }
                    

                dailySuggestion.Places.AddRange(lunchPlace);

                var afternoonActivities = GetRandomPlaces(
                    allPlacesByCategory.GetValueOrDefault("parks", new List<PlaceSuggestion>())
                        .Concat(allPlacesByCategory.GetValueOrDefault("shopping_mall", new List<PlaceSuggestion>()))
                        .ToList(),
                    _random.Next(1, 3)
                );
                dailySuggestion.Places.AddRange(afternoonActivities);

                var dinnerPlaces = allPlacesByCategory.GetValueOrDefault("restaurants", new List<PlaceSuggestion>())
                    .Where(p => !lunchPlace.Any(lp => lp.GooglePlaceId == p.GooglePlaceId))
                    .ToList();
                var dinnerPlace = GetRandomPlaces(dinnerPlaces, 1);
                dailySuggestion.Places.AddRange(dinnerPlace);

                response.DailySuggestions.Add(dailySuggestion);
            }

            _logger.LogInformation($"Generated suggestions for {totalDays} days with a total of {response.DailySuggestions.Sum(d => d.Places.Count)} places");
            return response;
        }

        private List<PlaceSuggestion> GetRandomPlaces(List<PlaceSuggestion> places, int count)
        {
            if (places == null || places.Count == 0)
                return new List<PlaceSuggestion>();

            return places
                .OrderBy(x => _random.Next())
                .Take(Math.Min(count, places.Count))
                .ToList();
        }
    }
}