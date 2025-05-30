namespace TravelPlanner.Application.DTOs
{
    public class PlaceSuggestionsResponseDto
    {
        public List<DailyPlaceSuggestions> DailySuggestions { get; set; } = new();
    }
}