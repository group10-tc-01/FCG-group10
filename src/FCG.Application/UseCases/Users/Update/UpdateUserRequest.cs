using MediatR;

namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserRequest : IRequest<UpdateUserResponse>
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
