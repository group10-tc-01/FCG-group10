using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FcgBaseController : ControllerBase
    {
        protected IMediator _mediator;

        public FcgBaseController(IMediator mediator) => _mediator = mediator;
    }
}
