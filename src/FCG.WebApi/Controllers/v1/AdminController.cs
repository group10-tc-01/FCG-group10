using FCG.Application.UseCases.Admin.CreateUser;
using FCG.Application.UseCases.Admin.GetAllUsers;
using FCG.Application.UseCases.Admin.GetById;
using FCG.Application.UseCases.Admin.RoleManagement;
using FCG.Domain.Models.Pagination;
using FCG.WebApi.Attributes;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    [Route("api/v1/admin/users")]
    [ApiController]
    public class AdminController : FcgBaseController
    {
        public AdminController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetAllUsersResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [AuthenticatedAdmin]
        public async Task<IActionResult> GetUser([FromQuery] GetAllUserCaseRequest queryPagination)
        {
            var output = await _mediator.Send(queryPagination, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<PagedListResponse<GetAllUsersResponse>>.SuccesResponse(output));
        }

        [HttpGet(("{id}"))]
        [ProducesResponseType(typeof(ApiResponse<GetUserByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [AuthenticatedAdmin]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var query = new GetUserByIdRequest(id);
            var output = await _mediator.Send(query, CancellationToken.None).ConfigureAwait(false);
            return Ok(ApiResponse<GetUserByIdResponse>.SuccesResponse(output));
        }

        [HttpPatch("{id}/update-role")]
        [AuthenticatedAdmin]
        [ProducesResponseType(typeof(ApiResponse<RoleManagementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<RoleManagementResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserRole([FromRoute] Guid id, [FromBody] RoleManagementBodyRequest request, CancellationToken cancellationToken)
        {
            var input = new RoleManagementRequest(id, request.NewRole);
            var output = await _mediator.Send(input, cancellationToken).ConfigureAwait(false);
            return Ok(ApiResponse<RoleManagementResponse>.SuccesResponse(output));
        }

        [HttpPost]
        [AuthenticatedAdmin]
        [ProducesResponseType(typeof(ApiResponse<CreateUserByAdminResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateUserByAdminResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserByAdminRequest input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken).ConfigureAwait(false);
            return Created(string.Empty, ApiResponse<CreateUserByAdminResponse>.SuccesResponse(output));
        }
    }
}
