using Asp.Versioning;
using FCG.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi.Controllers.v1
{
    [ExcludeFromCodeCoverage]

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FcgBaseController : ControllerBase
    {
        protected IMediator _mediator;
        protected ICurrentUserService _currentUserService;
        public FcgBaseController(IMediator mediator) => _mediator = mediator;
        public FcgBaseController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }
    }
}
