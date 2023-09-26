using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using LanguageExt;
using LanguageExt.Common;

namespace SingularityInterview.APIs.Domain.Kernel.Shared.Exceptions;
internal class InvalidEmailException
    : ValidationException
{
    public static Option<InvalidEmailException> ReturnIfInvalid(
        string? email,
        [CallerArgumentExpression(nameof(email))] string? paramName = null)
    {
        Result<string> emailResult = Validate(email, paramName!);
        return emailResult.Match(
            Succ: (string _) => Option<InvalidEmailException>.None,
            Fail: (Exception ex) => Option<InvalidEmailException>.Some(new InvalidEmailException(ex)));
    }

    internal InvalidEmailException(string message)
        : base(message)
    {
    }

    internal InvalidEmailException(string message, string paramName)
        : base($"{message} (param name: {paramName})")
    {
    }

    private InvalidEmailException(Exception exception)
        : base(exception.Message, exception.InnerException)
    {
    }

    private static Result<string> Validate(string? email, string paramName)
    {
        if (email is null)
        {
            return new Result<string>(new InvalidEmailException(
                $"Email can not be null.",
                paramName));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return new Result<string>(new InvalidEmailException(
                $"Email can not be empty.",
                paramName));
        }

        if (MailAddress.TryCreate(email, out var address))
        {
            if (address is null)
            {
                return new Result<string>(new InvalidEmailException(
                $"Email can not be empty.",
                    paramName));
            }

            return new Result<string>(email);
        }

        return new Result<string>(e: new InvalidEmailException($"Inavlid email address. (param name: {paramName})"));
    }
}
