using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    public class HealthController : FcgBaseController
    {
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("API is healthy");
        }
    }
}
