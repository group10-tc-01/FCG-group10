using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Application.UseCases.Users.RoleManagement.RoleManagementDTO;
using FCG.WebApi.Attributes;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    public class UsersController : FcgBaseController
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterUserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest input)
        {
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);
            return Created(string.Empty, ApiResponse<RegisterUserResponse>.SuccesResponse(output));
        }

        [HttpPatch("admin/update-role")]
        [AuthenticatedAdmin]
        [ProducesResponseType(typeof(ApiResponse<RoleManagementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<RoleManagementResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserRole([FromBody] RoleManagementRequest input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return Ok(ApiResponse<RoleManagementResponse>.SuccesResponse(output));
        }

    }
}