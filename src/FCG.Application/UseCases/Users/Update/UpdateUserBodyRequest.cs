namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserBodyRequest
    {
        public string CurrentPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
    }
}