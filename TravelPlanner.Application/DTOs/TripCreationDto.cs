using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Application.DTOs
{
    public class TripCreationDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        [StringLength(100)]
        public required string Destination { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}