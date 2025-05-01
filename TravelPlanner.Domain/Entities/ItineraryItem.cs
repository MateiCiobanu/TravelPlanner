using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanner.Domain.Entities
{
    public class ItineraryItem
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public string GooglePlaceId { get; set; }
        public string PlaceName { get; set; }
        public string PlaceCategory { get; set; }
        public int DayNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Duration { get; set; } // minutes
        public string? ExternalInfoLink { get; set; }
    }
}
