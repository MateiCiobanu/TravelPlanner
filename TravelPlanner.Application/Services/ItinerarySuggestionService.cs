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
            _logger.LogWarning($"Received traveler type: {request.TravelerType?.TravelerTypeName}");
            var response = new PlaceSuggestionsResponseDto();
            
            int totalDays = (int)(request.EndDate - request.StartDate).TotalDays + 1;
            _logger.LogInformation($"Trip duration: {totalDays} days");


            var preferredCategories = new List<string>();

            var travelerTypeName = request.TravelerType?.TravelerTypeName?.ToLowerInvariant() ?? "";

            switch (travelerTypeName)
            {
                case "culture explorer":
                    preferredCategories = new List<string> {    "museum", "art_gallery", "church",
                                                                "synagogue", "mosque", "library",
                                                                "book_store", "tourist_attraction", "university",
                                                                "local_government_office" };
                    break;
                case "chill seeker":
                    preferredCategories = new List<string> {    "cafe", "spa", "church",
                                                                "hindu_temple", "mosque", "synagogue",
                                                                "park", "art_gallery", "library",
                                                                "movie_theater" };
                    break;
                case "night owl":
                    preferredCategories = new List<string> {    "bar", "night_club", "pub",
                                                                "casino", "restaurant", "meal_takeaway",
                                                                "movie_theater", "bowling_alley" };
                    break;
                case "foodie adventurer":
                    preferredCategories = new List<string> {    "restaurant", "cafe", "bakery",
                                                                "bar", "meal_takeaway", "meal_delivery",
                                                                "supermarket", "grocery_or_supermarket",
                                                                "night_club" };
                    break;
                case "urban explorer":
                    preferredCategories = new List<string> {    "cafe", "shopping_mall", "clothing_store",
                                                                "book_store", "bar", "night_club",
                                                                "tourist_attraction", "movie_theater", "convenience_store",
                                                                "art_gallery" };
                    break;
                case "nature enthusiast":
                    preferredCategories = new List<string> {    "park", "campground", 
                                                                "scenic_lookout", "zoo",
                                                                "aquarium", "gym", "spa",
                                                                };
                    break;
                default:
                    preferredCategories = new List<string>
                    {
                        // Food & Drink
                        "restaurant", "cafe", "bar", "bakery",
                        // Entertainment & Nightlife
                        "movie_theater", "night_club", "bowling_alley", "casino",
                        // Culture & Education
                        "museum", "art_gallery", "library", "church", "synagogue", "mosque",
                        // Nature & Leisure
                        "park", "zoo", "aquarium", "campground", "spa",
                        // Shopping & Exploration
                        "shopping_mall", "clothing_store", "book_store", "convenience_store", "supermarket",
                        // Tourist Hotspots
                        "tourist_attraction", "local_government_office", "university"
                    };
                    break;
            }

            var allPlacesByCategory = new Dictionary<string, List<PlaceSuggestion>>();
            
            foreach (var category in preferredCategories)
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

                // Randomly pick morning, lunch, afternoon, dinner from allowed categories
                foreach (var slot in new[] { "morning", "lunch", "afternoon", "dinner" })
                {
                    var shuffledCategories = preferredCategories.OrderBy(c => _random.Next()).ToList();
                    bool added = false;

                    foreach (var category in shuffledCategories)
                    {
                        var pool = allPlacesByCategory.GetValueOrDefault(category, new List<PlaceSuggestion>());
                        if (pool == null || pool.Count == 0) continue;

                        var neededCount = slot == "lunch" || slot == "dinner" ? 1 : _random.Next(1, 3);
                        var selected = GetRandomPlaces(pool, neededCount);

                        if (selected.Any())
                        {
                            dailySuggestion.Places.AddRange(selected);
                            added = true;
                            break;
                        }
                    }

                    if (!added)
                    {
                        _logger.LogWarning($"No suitable activity found for {slot} on Day {day} for type '{travelerTypeName}'");
                    }
                }

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