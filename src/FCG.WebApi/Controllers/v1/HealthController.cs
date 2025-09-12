using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    public class HealthController : FcgBaseController
    {
        public HealthController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("API is healthy");
        }
    }
}
