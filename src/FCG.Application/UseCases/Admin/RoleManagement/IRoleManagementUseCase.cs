using MediatR;

namespace FCG.Application.UseCases.Admin.RoleManagement
{
    public interface IRoleManagementUseCase : IRequestHandler<RoleManagementRequest, RoleManagementResponse> { }
}
