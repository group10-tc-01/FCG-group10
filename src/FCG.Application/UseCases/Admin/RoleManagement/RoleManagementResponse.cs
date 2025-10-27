using FCG.Domain.Enum;

namespace FCG.Application.UseCases.Admin.RoleManagement
{
    public record RoleManagementResponse(Guid UserId, string UserName, string UserEmail, Role Role);
}
