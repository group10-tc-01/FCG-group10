using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;

namespace FCG.Infrastructure.Services.CurrentUser
{
    public class CurrentUserService(IReadOnlyUserRepository userRepository) : ICurrentUserService
    {
        public Guid UserId { get; set; }
        public async Task<Domain.Entities.User?> GetUserAsync()
        {
            return await userRepository.GetByIdAsync(UserId);
        }
    }
}
