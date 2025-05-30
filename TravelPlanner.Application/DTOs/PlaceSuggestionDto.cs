namespace TravelPlanner.Application.DTOs
{
    public class PlaceSuggestion
    {
        public required string GooglePlaceId { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required string Address { get; set; }
        public double Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Schedule { get; set; }
        public List<string> OpeningHours { get; set; } = new();
        public string? TodayHours { get; set; }

        public string? FormattedPhoneNumber { get; set; }
        public string? InternationalPhoneNumber { get; set; }
        public string? Website { get; set; }
    }
}