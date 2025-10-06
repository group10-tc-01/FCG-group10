namespace FCG.Application.UseCases.Users.Update.UsersDTO
{
    public class UpdateUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } = string.Empty;
    }
}
