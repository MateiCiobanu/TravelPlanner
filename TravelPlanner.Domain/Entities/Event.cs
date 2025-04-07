using System;

namespace TravelPlanner.Domain.Entities;

public class Event
{
    public string EventName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime StartDateAndTime { get; set; }
    public DateTime EndDateAndTime { get; set; }
}