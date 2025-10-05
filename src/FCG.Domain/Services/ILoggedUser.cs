using FCG.Domain.Entities;

namespace FCG.Domain.Services
{
    public interface ILoggedUser
    {
        Task<User> GetLoggedUserAsync();
    }
}
