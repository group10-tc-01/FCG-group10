using System;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public string Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public Wallet Wallet { get; set; }
        public Library Library { get; set; }

        protected User() { }

        public User(string name, Email email, Password password, string role)
        {


            Name = name;
            Email = email;
            Password = password;
            Role = role;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}