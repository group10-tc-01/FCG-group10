using FCG.Domain.Enum;

namespace FCG.Application.UseCases.Users.RoleManagement.RoleManagementDTO
{
    public record RoleManagementResponse(Guid UserId, string UserName, string UserEmail, Role Role);
}
