namespace FCG.Application.UseCases.Users.GetAllUsers
{
    public class UserListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }
}
