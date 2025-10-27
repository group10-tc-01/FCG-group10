using FCG.Domain.Enum;

namespace FCG.Application.UseCases.Users.RoleManagement
{
    public record RoleManagementResponse(Guid UserId, string UserName, string UserEmail, Role Role);
}
