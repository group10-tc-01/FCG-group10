using FCG.Application.UseCases.Games.Register;
using FCG.WebApi.Attributes;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    public class GamesController(IMediator mediator) : FcgBaseController(mediator)
    {
        [HttpPost]
        [AuthenticatedAdmin]
        [ProducesResponseType(typeof(ApiResponse<RegisterGameOutput>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterGameInput input)
        {
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);
            return Created(string.Empty, ApiResponse<RegisterGameOutput>.SuccesResponse(output));
        }
    }
}
