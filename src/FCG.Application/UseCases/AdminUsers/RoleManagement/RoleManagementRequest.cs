using FCG.Domain.Enum;
using MediatR;

namespace FCG.Application.UseCases.AdminUsers.RoleManagement
{
    public record RoleManagementRequest(Guid UserId, Role NewRole) : IRequest<RoleManagementResponse>;
}
