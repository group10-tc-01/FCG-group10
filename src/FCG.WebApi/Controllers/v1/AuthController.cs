using FCG.Application.UseCases.Auth.Login;
using FCG.Application.UseCases.Auth.RefreshToken;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    public class AuthController : FcgBaseController
    {
        public AuthController(IMediator mediator) : base(mediator) { }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginInput input)
        {
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);

            return Ok(ApiResponse<LoginOutput>.SuccesResponse(output));
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<RefreshTokenOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenInput input)
        {
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);

            return Ok(ApiResponse<RefreshTokenOutput>.SuccesResponse(output));
        }
    }
}
