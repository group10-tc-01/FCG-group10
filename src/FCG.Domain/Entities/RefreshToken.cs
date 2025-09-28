namespace FCG.Domain.Entities
{
    public sealed class RefreshToken : BaseEntity
    {
        public string Token { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public string RevokedReason { get; private set; } = string.Empty;

        public User? User { get; }

        private RefreshToken(string token, Guid userId, DateTime expiresAt)
        {
            Token = token;
            UserId = userId;
            ExpiresAt = expiresAt;
        }

        public static RefreshToken Create(string token, Guid userId, TimeSpan expiration)
        {
            return new RefreshToken(token, userId, DateTime.UtcNow.Add(expiration));
        }

        public void Revoke(string reason = "New refresh token generated")
        {
            IsActive = false;
            RevokedReason = reason;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsValid => IsActive && ExpiresAt > DateTime.UtcNow;
    }
}