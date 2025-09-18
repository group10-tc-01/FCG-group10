using FCG.Domain.Exceptions;
using System.Net.Mail;

namespace FCG.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Email cannot be null or empty.");

            if (!IsValidEmail(value))
                throw new DomainValidationException("Invalid email format.");

            if (value.Length > 255)
                throw new DomainValidationException("Email cannot be longer than 255 characters.");

            return new Email(value);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static implicit operator string(Email email) => email.Value;
        public static implicit operator Email(string value) => Create(value);

        public override string ToString() => Value;
    }
}
