using FCG.Application.UseCases.Users.RoleManagement.RoleManagementDTO;
using MediatR;

namespace FCG.Application.UseCases.Users.RoleManagement
{
    public interface IRoleManagementUseCase : IRequestHandler<RoleManagementRequest, RoleManagementResponse> { }
}
