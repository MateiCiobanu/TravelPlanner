using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TravelPlanner.WebUI.Pages
{
    public class ItineraryModel : PageModel
    {
        public Dictionary<int, List<string>> ActivitiesByDay { get; set; } = new();

        public void OnGet()
        {
            ActivitiesByDay = new Dictionary<int, List<string>>
            {
                { 1, new List<string> { "Visit Colosseum", "Lunch near Trevi Fountain", "Evening walk at Spanish Steps" } },
                { 2, new List<string> { "Vatican Museum tour", "Piazza Navona photoshoot", "Trastevere dinner" } },
                { 3, new List<string> { "Borghese Gallery", "Bike tour in Villa Borghese", "Sunset over Pincian Hill" } },
                { 4, new List<string> { "Free exploration", "Souvenir shopping", "Airport transfer" } }
            };
        }
    }
}
