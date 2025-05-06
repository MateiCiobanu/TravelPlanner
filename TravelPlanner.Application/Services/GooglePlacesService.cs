using TravelPlanner.Application.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TravelPlanner.Application.Services;

public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;
  private readonly ILogger<ItinerarySuggestionService> _logger;


    public GooglePlacesService(HttpClient httpClient, IConfiguration configuration
)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GooglePlaces:ApiKey"]; 


    }

    public async Task<List<PlaceDto>> GetPlacesAsync(string query, string location)
    {
          query = Uri.EscapeDataString(query);
        location = Uri.EscapeDataString(location);
        var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={query}&location={location}&radius=50000&key={_apiKey}";

        var response = await _httpClient.GetFromJsonAsync<JsonElement>(url);
        var places = new List<PlaceDto>();

        foreach (var result in response.GetProperty("results").EnumerateArray())
        {
                    places.Add(new PlaceDto
            {
                Name = result.GetProperty("name").GetString() ?? "Unknown",
                PlaceId = result.GetProperty("place_id").GetString() ?? "",
                Address = result.GetProperty("formatted_address").GetString() ?? "",
                Website = null,
                FormattedPhoneNumber = null,
                Rating = result.TryGetProperty("rating", out var rating) ? rating.GetDouble() : 0,
                Latitude = result.GetProperty("geometry").GetProperty("location").GetProperty("lat").GetDouble(),
                Longitude = result.GetProperty("geometry").GetProperty("location").GetProperty("lng").GetDouble(),
                Types = new List<string>(), // or get it from result if available
                OpeningHours = new List<string>(), // same here
            });

        }

        return places;
    }

    public async Task<PlaceDetailsDto> GetPlaceDetailedInfoAsync(string place_id)
    {
        var url = $"https://places.googleapis.com/v1/places/{place_id}?languageCode=en&key={_apiKey}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Goog-FieldMask", "*");

        var httpResponse = await _httpClient.SendAsync(request);
        httpResponse.EnsureSuccessStatusCode();

        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<JsonElement>();

        var location = jsonResponse.GetProperty("location");

        var details = new PlaceDetailsDto(
            PlaceId: jsonResponse.GetProperty("id").GetString() ?? "",
            Name: jsonResponse.GetProperty("displayName").GetProperty("text").GetString() ?? "",
            FormattedAddress: jsonResponse.GetProperty("formattedAddress").GetString() ?? "",
            NationalPhoneNumber: jsonResponse.TryGetProperty("nationalPhoneNumber", out var nationalPhoneNumber) ? nationalPhoneNumber.GetString() : null,
            InternationalPhoneNumber: jsonResponse.TryGetProperty("internationalPhoneNumber", out var internationalPhoneNumber) ? internationalPhoneNumber.GetString() : null,
            WebsiteUri: jsonResponse.TryGetProperty("websiteUri", out var websiteUri) ? websiteUri.GetString() : null,
            Rating: jsonResponse.TryGetProperty("rating", out var rating) ? rating.GetDouble() : null,
            UserRatingCount: jsonResponse.TryGetProperty("userRatingCount", out var userRatingCount) ? userRatingCount.GetInt32() : 0,
            GoogleMapsUri: jsonResponse.GetProperty("googleMapsUri").GetString() ?? "",
            Location: new LocationDto(
                Latitude: location.GetProperty("latitude").GetDouble(),
                Longitude: location.GetProperty("longitude").GetDouble()
            ),
            OpeningHours: jsonResponse.TryGetProperty("regularOpeningHours", out var openingHours) &&
                            openingHours.TryGetProperty("weekdayDescriptions", out var descriptions)
                            ? descriptions.EnumerateArray().Select(d => d.GetString() ?? "").ToList()
                            : new List<string>()
        );

        return details;
    }
}