using FCG.Domain.Enum;
using MediatR;

namespace FCG.Application.UseCases.Admin.RoleManagement
{
    public record RoleManagementRequest(Guid UserId, Role NewRole) : IRequest<RoleManagementResponse>;
}
