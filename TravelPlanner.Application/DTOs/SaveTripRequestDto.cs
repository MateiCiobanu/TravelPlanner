
namespace TravelPlanner.Application.DTOs
{
    public class SaveTripRequestDto
    {
        public int UserId { get; set; }
        public string? Title { get; set; }
        public required string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ItineraryItemDto> ItineraryItems { get; set; } = new();
        public List<string> FriendEmails { get; set; } = new();
    }
}