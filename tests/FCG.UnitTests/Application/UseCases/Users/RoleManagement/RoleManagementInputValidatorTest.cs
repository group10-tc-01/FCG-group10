using FCG.Application.UseCases.Admin.RoleManagement;
using FCG.CommomTestsUtilities.Builders.Inputs.RoleManagement;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Users.RoleManagement
{
    public class RoleManagementInputValidatorTest
    {
        [Fact]
        public void Given_ValidInput_When_Validate_Then_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = RoleManagementInputBuilder.Build();
            var validator = new RoleManagementInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Given_InputWithEmptyUserId_When_Validate_Then_ShouldHaveValidationErrorForUserId()
        {
            // Arrange
            var input = RoleManagementInputBuilder.BuildWithEmptyUserId();
            var validator = new RoleManagementInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessages.UserIdRequired);
        }

        [Fact]
        public void Given_InputWithInvalidRole_When_Validate_Then_ShouldHaveValidationErrorForRole()
        {
            // Arrange
            var input = RoleManagementInputBuilder.BuildWithInvalidRole();
            var validator = new RoleManagementInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessages.InvalidUserRole);
        }
    }
}