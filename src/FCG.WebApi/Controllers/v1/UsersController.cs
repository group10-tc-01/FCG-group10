using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Application.UseCases.Users.Update.UsersDTO;
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

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var output = await _mediator.Send(request, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<UpdateUserResponse>.SuccesResponse(output));
        }
    }
}
