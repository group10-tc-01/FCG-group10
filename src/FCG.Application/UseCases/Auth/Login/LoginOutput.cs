namespace FCG.Application.UseCases.Auth.Login
{
    public class LoginOutput
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public int ExpiresIn { get; init; }
    }
}
