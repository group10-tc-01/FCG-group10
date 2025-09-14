using FCG.Domain.EntityBase;

namespace FCG.Domain.EntityUser
{
    public class EntityUser : Base
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public EntityUser(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
