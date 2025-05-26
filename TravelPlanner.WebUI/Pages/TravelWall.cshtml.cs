using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TravelPlanner.WebUI.Pages
{
    public class TravelWallModel : PageModel
    {
        public List<TravelPost> Posts { get; set; }

        public void OnGet()
        {
            Posts = new List<TravelPost>
            {
                new TravelPost
                {
                    Username = "Anna M.",
                    TravelerType = "Urban Explorer",
                    PostDate = "April 14, 2025 – 10:32 AM",
                    Location = "Rome, Italy",
                    ImagePath = "media/bg13.jpg",
                    Comments = new List<string>
                    {
                        "Such a beautiful place! – Marco",
                        "I was there last summer! – Laura",
                        "Love this shot! – Elena"
                    }
                },
                new TravelPost
                {
                    Username = "Leo B.",
                    TravelerType = "Nature Enthusiast",
                    PostDate = "April 13, 2025 – 6:45 PM",
                    Location = "Banff, Canada",
                    ImagePath = "media/bg12.jpg",
                    Comments = new List<string>
                    {
                        "Stunning view! – Emma",
                        "Nature is healing 🌿 – Chris"
                    }
                }
            };
        }
    }

   public class TravelPost
{
    public string? Username { get; set; }
    public string? TravelerType { get; set; }
    public string? PostDate { get; set; }
    public string? Location { get; set; }
    public string? ImagePath { get; set; }
    public List<string>? Comments { get; set; }
}

}
