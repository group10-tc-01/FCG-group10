using FCG.Domain.Entities;

namespace FCG.Domain.Services
{
    public interface ICurrentUserService
    {
        Guid UserId { get; set; }
        Task<User?> GetUserAsync();
    }
}
