namespace FCG.Application.UseCases.Authentication.RefreshToken
{
    public class RefreshTokenOutput
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public int ExpiresInDays { get; init; }
    }
}