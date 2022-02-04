using Microsoft.AspNetCore.Mvc;

namespace UpstreamService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeController : ControllerBase
    {
        [HttpGet("now")]
        public ActionResult<string> TimeNow() => DateTime.UtcNow.ToString();
    }
}
