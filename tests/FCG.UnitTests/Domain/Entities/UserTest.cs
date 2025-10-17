using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void Given_ValidUserParameters_When_Create_Then_UserIsInstantiatedCorrectly()
        {

            var userBuilder = UserBuilder.Build();

            var rawPassword = userBuilder.Password;
            var user = User.Create(userBuilder.Name, userBuilder.Email, rawPassword, Role.Admin);


            user.Should().NotBeNull();
            user.Id.Should().NotBe(Guid.Empty);
            user.Name.Should().Be(userBuilder.Name);
            user.Email.Should().Be(userBuilder.Email);
        }


        [Fact]
        public void Given_TwoUsersWithSameData_When_Created_Then_TheyHaveDifferentIds()
        {
            // Arrange
            var userBuilder = UserBuilder.Build();

            // Act
            var user1 = User.Create(userBuilder.Name, userBuilder.Email, userBuilder.Password, Role.Admin);
            var user2 = User.Create(userBuilder.Name, userBuilder.Email, userBuilder.Password, Role.Admin);

            // Assert
            user1.Id.Should().NotBe(user2.Id);
        }

        [Fact]
        public void Given_InvalidName_When_CreateUser_Then_ThrowsException()
        {
            // Arrange
            var userBuilder = UserBuilder.Build();

            // Act
            Action act = () => User.Create(null, userBuilder.Email, userBuilder.Password, Role.Admin);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Name cannot be null or empty.");
        }

        [Fact]
        public void Given_InvalidEmail_When_CreateUser_Then_ThrowsException()
        {
            // Arrange
            var userBuilder = UserBuilder.Build();

            // Act
            Action act = () => User.Create(userBuilder.Name, "invalid-email", userBuilder.Password, Role.Admin);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Invalid email format.");
        }

    }

}
