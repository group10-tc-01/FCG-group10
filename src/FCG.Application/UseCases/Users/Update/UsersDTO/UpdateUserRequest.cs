using MediatR;

namespace FCG.Application.UseCases.Users.Update.UsersDTO
{
    public class UpdateUserRequest : IRequest<UpdateUserResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;



    }
}
