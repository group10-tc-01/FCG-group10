using MediatR;

namespace FCG.Application.UseCases.AdminUsers.RoleManagement
{
    public interface IRoleManagementUseCase : IRequestHandler<RoleManagementRequest, RoleManagementResponse> { }
}
