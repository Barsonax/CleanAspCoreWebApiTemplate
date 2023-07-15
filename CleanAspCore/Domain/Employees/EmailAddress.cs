namespace CleanAspCore.Domain.Employees;

public record EmailAddress
{
    public string Email { get; }

    public EmailAddress(string email)
    {
        Email = email;
        Validator.Instance.ValidateAndThrow(this);
    }

    public static bool operator ==(EmailAddress emailAddress, string email) => emailAddress.Equals(email);

    public static bool operator !=(EmailAddress emailAddress, string email) => !emailAddress.Equals(email);
    
    public bool Equals(string email) => Email == email;

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Email;

    public static implicit operator EmailAddress(string emailAddress) => new(emailAddress);

    public override string ToString() => Email;

    private class Validator : AbstractValidator<EmailAddress>
    {
        public static readonly Validator Instance = new();

        private Validator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
        }
    }
}