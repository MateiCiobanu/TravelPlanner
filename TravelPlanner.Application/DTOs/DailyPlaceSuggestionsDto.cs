namespace TravelPlanner.Application.DTOs
{
    public class DailyPlaceSuggestions
    {
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }
        public List<PlaceSuggestion> Places { get; set; } = new List<PlaceSuggestion>();
    }
}