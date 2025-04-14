
using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Application.DTOs
{

// Trip-related DTOs
    public class TripCreationDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Destination { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
    }
    
    public class TripDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public List<ItineraryItemDto> ItineraryItems { get; set; }
    }
    
    public class ItineraryItemDto
    {
        public int Id { get; set; }
        public string GooglePlaceId { get; set; }
        public string PlaceName { get; set; }
        public string PlaceCategory { get; set; }
        public int DayNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Duration { get; set; }
    }
    
    public class PlaceSuggestionRequestDto
    {
        [Required]
        public string Destination { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
    }
    
    public class PlaceSuggestionsResponseDto
    {
        public List<DailyPlaceSuggestions> DailySuggestions { get; set; } = new List<DailyPlaceSuggestions>();
    }
    
    public class DailyPlaceSuggestions
    {
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }
        public List<PlaceSuggestion> Places { get; set; } = new List<PlaceSuggestion>();
    }
    
    public class PlaceSuggestion
    {
        public string GooglePlaceId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        

    }

}