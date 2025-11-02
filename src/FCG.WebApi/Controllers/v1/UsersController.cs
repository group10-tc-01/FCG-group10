using FCG.Application.UseCases.Users.Register;
using FCG.Application.UseCases.Users.Register.UsersDTO.FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Application.UseCases.Users.Update;
using FCG.WebApi.Attributes;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace FCG.WebApi.Controllers.v1
{
    public class UsersController : FcgBaseController
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RegisterUserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest input)
        {
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);
            return Created(string.Empty, ApiResponse<RegisterUserResponse>.SuccesResponse(output));
        }

        [HttpPut("{id}")]
        [AuthenticatedUser]
        [ProducesResponseType(typeof(ApiResponse<UpdateUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserBodyRequest bodyRequest)
        {
            var request = new UpdateUserRequest(id, bodyRequest);
            var output = await _mediator.Send(request, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<UpdateUserResponse>.SuccesResponse(output));
        }

    }
}
