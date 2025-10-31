using MediatR;

namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserRequest : IRequest<UpdateUserResponse>
    {
        public Guid Id { get; init; }
        public string CurrentPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;

        public UpdateUserRequest(UpdateUserBodyRequest bodyRequest)
        {
            CurrentPassword = bodyRequest.CurrentPassword;
            NewPassword = bodyRequest.NewPassword;
        }
    }
}
