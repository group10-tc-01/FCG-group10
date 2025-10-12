namespace FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO
{
    public class LibraryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
