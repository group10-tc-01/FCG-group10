namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserResponse
    {
        public Guid Id { get; init; }
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    }
}
