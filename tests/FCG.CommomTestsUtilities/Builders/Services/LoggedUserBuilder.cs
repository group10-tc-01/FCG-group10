using FCG.Domain.Entities;
using FCG.Domain.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public class LoggedUserBuilder
    {
        private static Mock<ILoggedUser> _mock;

        public static ILoggedUser Build()
        {
            _mock = new Mock<ILoggedUser>();
            return _mock.Object;
        }

        public static void SetupGetLoggedUserAsync(User user)
        {
            _mock.Setup(x => x.GetLoggedUserAsync())
                .ReturnsAsync(user);
        }
    }
}