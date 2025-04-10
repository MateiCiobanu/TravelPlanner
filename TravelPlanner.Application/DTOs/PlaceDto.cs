namespace TravelPlanner.Application.DTOs;

public record class PlaceDto(
    string Name,
    string PlaceId,
    string Address,
    string? PhoneNumber,
    string? Website,
    double Rating,
    int UserRatingsTotal,
    string GoogleMapsUrl,
    LocationDto Location
);
