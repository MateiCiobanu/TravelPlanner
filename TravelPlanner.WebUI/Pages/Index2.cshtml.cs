// using Microsoft.AspNetCore.Mvc.RazorPages;
// using System.Net.Http;
// using System.Threading.Tasks;

// namespace TravelPlanner.WebUI.Pages
// {
//     public class IndexModel : PageModel
//     {
//         private readonly IHttpClientFactory _httpClientFactory;

//         public string MessageFromApi { get; set; } = "Click the button to fetch data.";

//         public IndexModel(IHttpClientFactory httpClientFactory)
//         {
//             _httpClientFactory = httpClientFactory;
//         }

//         public async Task OnPostFetchData()
//         {
//             var client = _httpClientFactory.CreateClient();
//             MessageFromApi = await client.GetStringAsync("http://localhost:5192/api/hello");
//         }
//     }
// }