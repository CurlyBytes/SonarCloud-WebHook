using Microsoft.AspNetCore.Mvc;

namespace SonarWebHookFrontAPI.Controllers
{
    [Produces("application/json")]
    [Route("Probe")]
    public class ProbeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}