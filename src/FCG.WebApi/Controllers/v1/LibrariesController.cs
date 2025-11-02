using FCG.Application.UseCases.Library.GetMyLibrary;
using FCG.WebApi.Attributes;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    [Route("api/v1/libraries")]
    [ApiController]
    public class LibrariesController : FcgBaseController
    {
        public LibrariesController(IMediator mediator) : base(mediator) { }

        [HttpGet("my-library")]
        [AuthenticatedUser]
        [ProducesResponseType(typeof(ApiResponse<GetMyLibraryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyLibrary(CancellationToken cancellationToken)
        {
            var request = new GetMyLibraryRequest();
            var output = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return Ok(ApiResponse<GetMyLibraryResponse>.SuccesResponse(output));
        }
    }
}
