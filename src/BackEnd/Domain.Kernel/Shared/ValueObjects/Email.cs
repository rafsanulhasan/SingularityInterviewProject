using System.Net.Mail;
using LanguageExt;
using LanguageExt.Common;
using OneOf;
using SingularityInterview.APIs.Domain.Kernel.Shared.Exceptions;

namespace SingularityInterview.APIs.Domain.Kernel.Shared.ValueObjects;

public class Email
    : OneOfBase<OptionalResult<string>>
{
    private readonly OptionalResult<string> _email;

    private Email(OneOf<OptionalResult<string>> email)
        : base(email)
    {
        _email = email.Match(
            f0: (OptionalResult<string> optionalEmail) => optionalEmail);
    }

    public Option<Exception> Exception { get; private set; } = Option<Exception>.None;

    public static Email Create(string? email)
    {
        if (email is null || string.IsNullOrWhiteSpace(email))
        {
            return new Email(OptionalResult<string>.Optional(null!));
        }

        Email result;
        if (!MailAddress.TryCreate(email, out MailAddress? mailAddress))
        {
            InvalidEmailException e = new("Invalid email address");
            result = new(new OptionalResult<string>(e))
            {
                Exception = e,
            };
            return result;
        }

        result = new(OptionalResult<string>.Some(mailAddress.ToString()));
        return result;
    }

    public static implicit operator Email(string? email)
    {
        return Create(email);
    }

    public static implicit operator string(Email email)
    {
        return email.Match(
            f0: (OptionalResult<string> optionalEmail) => optionalEmail.Match(
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
    }
}
