namespace FCG.Domain.Entities
{
    public sealed class RefreshToken : BaseEntity
    {
        public string Token { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public string? RevokedReason { get; private set; }

        public User User { get; private set; } = null!;

        private RefreshToken() { }

        private RefreshToken(string token, Guid userId, DateTime expiresAt)
        {
            Token = token;
            UserId = userId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            IsRevoked = false;
        }

        public static RefreshToken Create(string token, Guid userId, TimeSpan expiration)
        {
            return new RefreshToken(token, userId, DateTime.UtcNow.Add(expiration));
        }

        public void Revoke(string reason = "Manual revocation")
        {
            IsRevoked = true;
            RevokedReason = reason;
        }

        public bool IsValid => !IsRevoked && ExpiresAt > DateTime.UtcNow;
    }
}