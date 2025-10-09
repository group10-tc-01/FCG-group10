using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;

namespace FCG.Application.UseCases.Users.RoleManagement.RoleManagementDTO
{
    public record RoleManagementResponse(Guid UserId, Name UserName, Email UserEmail, Role Role);
}
