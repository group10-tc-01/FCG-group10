using FCG.Application.UseCases.Authentication.Login;
using FCG.Application.UseCases.Authentication.Logout;
using FCG.Application.UseCases.Authentication.RefreshToken;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LogoutOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Ok("ERROR");

            var input = new LogoutInput { UserId = userId };
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);

            return Ok(ApiResponse<LogoutOutput>.SuccesResponse(output));
        }
    }
}
