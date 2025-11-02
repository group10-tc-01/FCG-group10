using FCG.Application.UseCases.Admin.CreateUser;
using FCG.Domain.Enum;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Admin.CreateUser
{
    public static class CreateUserByAdminInputBuilder
    {
        public static CreateUserByAdminRequest Build()
        {
            return new CreateUserByAdminRequest
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "StrongP@ssw0rd!123",
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithAdminRole()
        {
            return new CreateUserByAdminRequest
            {
                Name = "Admin User",
                Email = "admin.user@example.com",
                Password = "AdminP@ssw0rd!123",
                Role = Role.Admin
            };
        }

        public static CreateUserByAdminRequest BuildWithEmptyName()
        {
            return new CreateUserByAdminRequest
            {
                Name = string.Empty,
                Email = "test@example.com",
                Password = "ValidP@ssw0rd!123",
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithInvalidEmail()
        {
            return new CreateUserByAdminRequest
            {
                Name = "Test User",
                Email = "invalid-email",
                Password = "ValidP@ssw0rd!123",
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithEmptyEmail()
        {
            return new CreateUserByAdminRequest
            {
                Name = "Test User",
                Email = string.Empty,
                Password = "ValidP@ssw0rd!123",
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithWeakPassword()
        {
            return new CreateUserByAdminRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "weak",
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithEmptyPassword()
        {
            return new CreateUserByAdminRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = string.Empty,
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithLongName()
        {
            return new CreateUserByAdminRequest
            {
                Name = new string('A', 101), // 101 caracteres
                Email = "test@example.com",
                Password = "ValidP@ssw0rd!123",
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithLongPassword()
        {
            return new CreateUserByAdminRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = new string('A', 101), // 101 caracteres
                Role = Role.User
            };
        }

        public static CreateUserByAdminRequest BuildWithInvalidRole()
        {
            return new CreateUserByAdminRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "ValidP@ssw0rd!123",
                Role = (Role)999 // Role inválida
            };
        }
    }
}
