using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace TravelPlanner.WebUI.Pages
{
    public class ItineraryModel : PageModel
    {
        private readonly IConfiguration _config;
        public string GooglePlacesApiKey { get; set; }

        public ItineraryModel(IConfiguration config)
        {
            _config = config;
        }

        public void OnGet()
        {
            GooglePlacesApiKey = _config["GooglePlaces:ApiKey"];
        }
    }
}