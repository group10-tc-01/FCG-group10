namespace FCG.Domain.Services
{
    public interface IPasswordEncrypter
    {
        string Encrypt(string password);
        bool IsValid(string password, string hashedPassword);
    }
}
