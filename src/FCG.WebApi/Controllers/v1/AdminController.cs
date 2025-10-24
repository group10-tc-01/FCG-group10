using FCG.Application.Shared.Models;
using FCG.Application.UseCases.AdminUsers.GetAllUsers;
using FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO;
using FCG.Application.UseCases.AdminUsers.GetById;
using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FCG.Application.UseCases.AdminUsers.RoleManagement.RoleManagementDTO;
using FCG.WebApi.Attributes;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    [Route("api/v1/admin/users")]
    [ApiController]
    public class AdminController : FcgBaseController
    {
        public AdminController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser([FromQuery] GetAllUserCaseQuery queryPagination)
        {
            var output = await _mediator.Send(queryPagination, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<PagedListResponse<UserListResponse>>.SuccesResponse(output));
        }

        [HttpGet(("{id}"))]
        [ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var query = new GetByIdUserQuery(id);
            var output = await _mediator.Send(query, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<UserDetailResponse>.SuccesResponse(output));
        }

        [HttpPatch("update-role")]
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
