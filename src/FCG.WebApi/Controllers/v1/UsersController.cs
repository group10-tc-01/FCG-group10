using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using FCG.Application.UseCases.Users.GetAllUsers;
using Microsoft.AspNetCore.Authorization;

namespace FCG.WebApi.Controllers.v1
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> GetUser()
        {
            var query = new GetAllUserCaseQuery();
            var output = await _mediator.Send(query, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<List<UserListResponse>>.SuccesResponse(output));

        }
    }
}