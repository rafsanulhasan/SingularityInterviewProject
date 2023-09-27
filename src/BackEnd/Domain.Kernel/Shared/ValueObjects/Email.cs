using LanguageExt;
using LanguageExt.Common;
using OneOf;
using SingularityInterview.APIs.Domain.Kernel.Shared.Exceptions;

namespace SingularityInterview.APIs.Domain.Kernel.Shared.ValueObjects;
public class Email
    : OneOfBase<string, OptionalResult<string>>
{
    private readonly OptionalResult<string> _email;

    private Email(OneOf<string, OptionalResult<string>> email)
        : base(email)
    {
    }

    public Exception Exception { get; private set; }

    public static Email Create(string? email)
    {
        if (email is null)
        {
            return new Email(new OptionalResult<string>(
                e: new InvalidEmailException($"Email can't be null.")));
        }

        Email result = new(email);
        Option<InvalidEmailException> optionalException = InvalidEmailException.ReturnIfInvalid(result);

        return optionalException.Match(
            Some: (InvalidEmailException e) => new Email(new OptionalResult<string>(
                e)),
            None: () => result);
    }

    public static Email Create(Option<string> email)
    {
        Email address = email.Match(
            Some: (string mail) => string.IsNullOrWhiteSpace(mail)
                ? Create(null)
                : new Email(mail),
            None: () => Create(null));
        return address;
    }

    public static implicit operator Email(string? email)
    {
        return Create(email);
    }

    public static implicit operator string(Email email)
    {
        return email.Match(
            f0: (string mail) => mail,
            f1: (OptionalResult<string> optionalEmail) => optionalEmail.Match(
                Some: (string mail) => mail,
                None: () => string.Empty,
                Fail: (Exception ex) =>
                {
                    email.Exception = ex;
                    return string.Empty;
                }));
    }

    public override int GetHashCode()
    {
        return _email.Match(
            Some: (string mail) => mail.GetHashCode(),
            None: () => string.Empty.GetHashCode(),
            Fail: (Exception ex) => ex.GetHashCode());
    }

    public override string ToString()
    {
        return _email.Match(
            Some: (string mail) => mail,
            None: () => string.Empty,
            Fail: (Exception ex) =>
            {
                Exception = ex;
                return string.Empty;
            });
        ;
    }
}
