using MediatR;

namespace FCG.Application.UseCases.Users.Update.UsersDTO
{
    public class UpdateUserRequest : IRequest<UpdateUserResponse>
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
