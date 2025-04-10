namespace TravelPlanner.Application.DTOs;

public record class PlaceDetailsDto(
    string PlaceId,
    string Name,
    string FormattedAddress,
    string? NationalPhoneNumber,
    string? InternationalPhoneNumber,
    string? WebsiteUri,
    double? Rating,
    int? UserRatingCount,
    string GoogleMapsUri,
    LocationDto Location,
    List<string> OpeningHours
);
