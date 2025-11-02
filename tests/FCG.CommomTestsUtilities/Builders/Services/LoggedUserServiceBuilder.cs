using FCG.Domain.Entities;
using FCG.Domain.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public class LoggedUserServiceBuilder
    {
        private static readonly Mock<ILoggedUser> _loggedUserMock = new();

        public static ILoggedUser Build()
        {
            return _loggedUserMock.Object;
        }

        public static void SetupGetLoggedUserAsync(User user)
        {
            _loggedUserMock
                .Setup(x => x.GetLoggedUserAsync())
                .ReturnsAsync(user);
        }
    }
}
