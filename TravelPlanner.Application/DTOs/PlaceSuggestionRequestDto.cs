using System.ComponentModel.DataAnnotations;
namespace TravelPlanner.Application.DTOs
{
    public class PlaceSuggestionRequestDto
    {
        [Required]
        public required string Destination { get; set; }

        [Required]
        public required DateTime StartDate { get; set; }

        [Required]
        public required DateTime EndDate { get; set; }

        [Required]
        public TravelerTypeDto? TravelerType { get; set; } // HAS TO BE REQURIED AFTER IMPLEMENTING THE PROFILE PAGE SO WE CAN FETCH THE TYPE 
        
        public string? EstimatedHoursPerDay { get; set; }
    }
}