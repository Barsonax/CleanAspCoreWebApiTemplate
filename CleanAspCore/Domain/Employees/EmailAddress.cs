namespace CleanAspCore.Domain.Employees;

public record EmailAddress
{
    public string Email { get; }

    public EmailAddress(string email)
    {
        Email = email;
        Validator.Instance.ValidateAndThrow(this);
    }

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Email;

    public static implicit operator EmailAddress(string emailAddress) => new(emailAddress);
    
    private class Validator : AbstractValidator<EmailAddress>
    {
        public static readonly Validator Instance = new();

        private Validator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
        }
    }
}