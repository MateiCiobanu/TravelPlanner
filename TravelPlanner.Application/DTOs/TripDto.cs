namespace TravelPlanner.Application.DTOs
{
    public class TripDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
        public required string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Status { get; set; }
        public required List<ItineraryItemDto> ItineraryItems { get; set; }
    }
}