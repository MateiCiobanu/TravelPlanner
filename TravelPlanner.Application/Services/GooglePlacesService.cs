using TravelPlanner.Application.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace TravelPlanner.Application.Services;

public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "AIzaSyBcWFBjVShy_n8fw0Jv5WqArQVtqCmnhfQ";

    public GooglePlacesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PlaceDto>> GetPlacesAsync(string category, string location)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={category}+in+{location}&key={ApiKey}";
        var response = await _httpClient.GetFromJsonAsync<JsonElement>(url);

        var places = new List<PlaceDto>();

        foreach (var result in response.GetProperty("results").EnumerateArray())
        {
            places.Add(new PlaceDto(
                Name: result.GetProperty("name").GetString() ?? "Unknown",
                PlaceId: result.GetProperty("place_id").GetString() ?? "",
                Address: result.GetProperty("formatted_address").GetString() ?? "",
                PhoneNumber: null,
                Website: null,
                Rating: result.TryGetProperty("rating", out var rating) ? rating.GetDouble() : 0,
                UserRatingsTotal: result.TryGetProperty("user_ratings_total", out var urt) ? urt.GetInt32() : 0,
                GoogleMapsUrl: $"https://www.google.com/maps/place/?q=place_id:{result.GetProperty("place_id").GetString()}",
                Location: new LocationDto(
                    Latitude: result.GetProperty("geometry").GetProperty("location").GetProperty("lat").GetDouble(),
                    Longitude: result.GetProperty("geometry").GetProperty("location").GetProperty("lng").GetDouble()
                )
            ));
        }

        return places;
    }

    public async Task<PlaceDetailsDto> GetPlaceDetailedInfoAsync(string place_id)
    {
        var url = $"https://places.googleapis.com/v1/places/{place_id}?languageCode=en&key={ApiKey}";

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
            OpeningHours:   jsonResponse.TryGetProperty("regularOpeningHours", out var openingHours) && 
                            openingHours.TryGetProperty("weekdayDescriptions", out var descriptions)
                            ? descriptions.EnumerateArray().Select(d => d.GetString() ?? "").ToList()
                            : new List<string>()
        );

        return details;
    }
}