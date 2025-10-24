using FCG.Domain.Enum;

namespace FCG.Application.UseCases.AdminUsers.RoleManagement.RoleManagementDTO
{
    public record RoleManagementResponse(Guid UserId, string UserName, string UserEmail, Role Role);
}
