using FCG.Application.UseCases.Users.GetAllUsers;
using FCG.Application.UseCases.Users.GetAllUsers.GetAllUserDTO;
using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using FCG.Application.Shared.Models;
using FCG.Application.UseCases.Users.Update.UsersDTO;
using Microsoft.AspNetCore.Authorization;


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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromQuery] GetAllUserCaseQuery queryPagination)
        {
            var output = await _mediator.Send(queryPagination, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<PagedListResponse<UserListResponse>>.SuccesResponse(output));

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var output = await _mediator.Send(request, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<UpdateUserResponse>.SuccesResponse(output));
        }
    }
}