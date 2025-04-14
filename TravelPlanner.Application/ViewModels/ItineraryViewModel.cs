namespace TravelPlanner.Application.ViewModels
{
    public class ItineraryViewModel
    {
        public string Destination { get; set; }
        public List<DayViewModel> Days { get; set; } = new List<DayViewModel>();
    }

    public class DayViewModel
    {
        public DateTime Date { get; set; }
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();
    }

    public class EventViewModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }
        public string PlaceCategory { get; set; }
        public string Address { get; set; }
        public double? Rating { get; set; }
    }
}