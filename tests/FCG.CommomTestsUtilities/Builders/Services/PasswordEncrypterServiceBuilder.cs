using FCG.Domain.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public static class PasswordEncrypterServiceBuilder
    {
        private static readonly Mock<IPasswordEncrypter> _mock = new Mock<IPasswordEncrypter>();
        public static IPasswordEncrypter Build() => _mock.Object;

        public static void SetupEncrypt(string encryptedPassword)
        {
            _mock.Setup(service => service.Encrypt(It.IsAny<string>())).Returns(encryptedPassword);
        }

        public static void SetupIsValid(bool isValid)
        {
            _mock.Setup(service => service.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(isValid);
        }

    }
}
