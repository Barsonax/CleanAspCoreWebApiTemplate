using FluentValidation;

namespace CleanAspCore.Core.Common.ValueObjects;

public readonly record struct EmailAddress
{
    public string Email { get; }

    public EmailAddress(string email)
    {
        Email = email;
        EmailAddressValidator.Instance.ValidateAndThrow(this);
    }

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Email;

    public override string ToString() => Email;

    private sealed class EmailAddressValidator : AbstractValidator<EmailAddress>
    {
        public static readonly AbstractValidator<EmailAddress> Instance = new EmailAddressValidator();

        private EmailAddressValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
        }
    }
}
