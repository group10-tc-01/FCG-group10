using FCG.Domain.Entities;

namespace FCG.Application.UseCases.Users.GetById.GetUserDTO
{
    public class UserIdListResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
