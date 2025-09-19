using System.Net.Mail;

namespace FCG.Domain.ValueObjects
{
    public sealed record Email
    {
        public string Value { get; private set; }
        public Email(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new ArgumentException("Verifique o e-mail");
            }
        }
        public bool IsValid(string value)
        {

            try
            {
                var addr = new MailAddress(value);
                return addr.Address == value;
            }
            catch
            {
                return false;
            }

        }
        public static implicit operator string(Email email) => email.Value;
        public override string ToString() => Value;
    }
}
