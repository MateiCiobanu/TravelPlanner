using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TravelPlanner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceInfoController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlaceInfoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("wikipedia")]
        public async Task<IActionResult> GetWikipediaLink([FromQuery] string placeName)
        {
            if (string.IsNullOrWhiteSpace(placeName))
                return BadRequest("Place name is required.");

            string url = await GetWikipediaUrlAsync(placeName);

            if (url == null)
                return NotFound("No Wikipedia article found.");

            return Ok(new { ExternalInfoLink = url });
        }

        private async Task<string> GetWikipediaUrlAsync(string placeName)
        {
            var client = _httpClientFactory.CreateClient();
            string apiUrl = $"https://en.wikipedia.org/w/api.php?action=query&titles={Uri.EscapeDataString(placeName)}&format=json";

            var response = await client.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var page = doc.RootElement
                .GetProperty("query")
                .GetProperty("pages")
                .EnumerateObject()
                .FirstOrDefault();

            if (page.Value.TryGetProperty("missing", out _))
                return null;

            return $"https://en.wikipedia.org/wiki/{Uri.EscapeDataString(placeName.Replace(' ', '_'))}";
        }
    }
}
