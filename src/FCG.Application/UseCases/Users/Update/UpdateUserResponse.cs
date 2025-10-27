namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserResponse
    {
        public Guid Id { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } = string.Empty;
    }
}
