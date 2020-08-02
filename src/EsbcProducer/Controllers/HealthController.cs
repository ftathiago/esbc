using Microsoft.AspNetCore.Mvc;

namespace EsbcProducer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("It's alive!");
        }
    }
}
