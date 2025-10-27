using MediatR;

namespace FCG.Application.UseCases.Users.RoleManagement
{
    public interface IRoleManagementUseCase : IRequestHandler<RoleManagementRequest, RoleManagementResponse> { }
}
