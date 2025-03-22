using Microsoft.AspNetCore.Mvc;

namespace TravelPlanner.API.Controllers
{
    [ApiController]
    [Route("")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMessage()
        {
            return Ok("Hello from TravelPlanner API! 🚀");
        }
    }
}