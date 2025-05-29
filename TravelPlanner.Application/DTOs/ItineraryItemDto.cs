
namespace TravelPlanner.Application.DTOs
{
    public class ItineraryItemDto
    {
        public int Id { get; set; }
        public required string GooglePlaceId { get; set; }
        public required string PlaceName { get; set; }
        public string? PlaceCategory { get; set; }
        public int DayNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Duration { get; set; }
    }
}