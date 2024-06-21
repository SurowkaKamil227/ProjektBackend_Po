using Microsoft.AspNetCore.Mvc;

namespace MyRazorApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Hello from the API!" });
        }
    }
}