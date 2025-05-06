
 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 namespace TravelPlanner.Domain.Models
{
 public class Trip
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Destination { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "planning";
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<ItineraryItem> ItineraryItems { get; set; }
    }

}
