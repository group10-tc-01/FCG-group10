using FCG.Application.Shared.Models;
using FCG.Application.UseCases.AdminUsers.GetAllUsers;
using FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO;
using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    [Route("api/[controller]/users")]
    [ApiController]
    public class AdminController : FcgBaseController
    {
        public AdminController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromQuery] GetAllUserCaseQuery queryPagination)
        {
            var output = await _mediator.Send(queryPagination, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<PagedListResponse<UserListResponse>>.SuccesResponse(output));

        }


        [HttpGet(("{id}"))]
        [ProducesResponseType(typeof(ApiResponse<GetUserByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var query = new GetByIdUserQuery(id);
            var output = await _mediator.Send(query, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<GetUserByIdResponse>.SuccesResponse(output));
        }
    }
}
