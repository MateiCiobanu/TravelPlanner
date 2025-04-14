 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 namespace TravelPlanner.Domain.Models
{
 public class ItineraryItem
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string GooglePlaceId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string PlaceName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PlaceCategory { get; set; }
        
        public int DayNumber { get; set; }
        
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Duration { get; set; } // in minutes
        
        // Navigation property
        public virtual Trip Trip { get; set; }
    }

}