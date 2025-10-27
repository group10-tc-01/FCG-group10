using FCG.Application.UseCases.AdminUsers.RoleManagement;
using FCG.Domain.Enum;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Users.RoleManagement
{
    public static class RoleManagementInputBuilder
    {
        public static RoleManagementRequest Build()
        {
            return new RoleManagementRequest(Guid.NewGuid(), Role.User);
        }

        public static RoleManagementRequest BuildWithEmptyUserId()
        {
            return new RoleManagementRequest(Guid.Empty, Role.User);
        }

        public static RoleManagementRequest BuildWithInvalidRole()
        {
            return new RoleManagementRequest(Guid.NewGuid(), (Role)999); // role inválido
        }
    }
}