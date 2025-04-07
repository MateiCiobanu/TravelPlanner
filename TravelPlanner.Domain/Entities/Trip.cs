using System;

namespace TravelPlanner.Domain.Entities;

public class Trip{
    public string CityName { get; private set; } = string.Empty;
    public DateOnly StarDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public List<Event> Events { get; private set; } = new List<Event>();
}