namespace TravelPlanner.Application.DTOs
{
    public class UpdateProfileDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TravelerType { get; set; } = string.Empty;
    }
}