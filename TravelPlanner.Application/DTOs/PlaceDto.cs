namespace TravelPlanner.Application.DTOs;
public class PlaceDto
{
    public required string PlaceId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public double Rating { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<string>? Types { get; set; }
    public string? FormattedPhoneNumber { get; set; }
    public string? Website { get; set; }
    public bool? OpenNow { get; set; }
    public List<string>? OpeningHours { get; set; }
}
