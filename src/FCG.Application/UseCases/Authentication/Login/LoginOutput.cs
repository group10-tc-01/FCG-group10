namespace FCG.Application.UseCases.Authentication.Login
{
    public class LoginOutput
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public int ExpiresInMinutes { get; init; }
    }
}
