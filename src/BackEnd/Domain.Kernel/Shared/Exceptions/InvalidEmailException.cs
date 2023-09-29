using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using LanguageExt.Common;

namespace SingularityInterview.APIs.Domain.Kernel.Shared.Exceptions;
public sealed class InvalidEmailException
    : ValidationException
{
    public static Result<string> ReturnIfInvalid(
        string? email,
        [CallerArgumentExpression(nameof(email))] string? paramName = null)
    {
        Result<string> emailResult = Validate(email, paramName!);
        return emailResult;
    }

    internal InvalidEmailException()
        : base("The email is invalid.")
    {
    }

    internal InvalidEmailException(string paramName)
        : base($"The email is invalid (param name: {paramName}).")
    {
    }

    private InvalidEmailException(Exception exception)
        : base(exception.Message, exception.InnerException)
    {
    }

    private static Result<string> Validate(string? email, string paramName)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return new Result<string>(string.Empty);
        }

        if (MailAddress.TryCreate(email, out MailAddress? address))
        {
            return new Result<string>(address.ToString());
        }

        return new Result<string>(e: new InvalidEmailException(paramName));
    }
}
