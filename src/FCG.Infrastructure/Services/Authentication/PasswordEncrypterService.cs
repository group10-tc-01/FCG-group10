using FCG.Domain.Services;
using static BCrypt.Net.BCrypt;

namespace FCG.Infrastructure.Services.Authentication
{
    public class PasswordEncrypterService : IPasswordEncrypter
    {
        public string Encrypt(string password)
        {
            return HashPassword(password);
        }

        public bool IsValid(string password, string hashedPassword)
        {
            return Verify(password, hashedPassword);
        }
    }
}
