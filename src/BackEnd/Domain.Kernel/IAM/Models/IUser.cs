using LanguageExt;
using SingularityInterview.APIs.Domain.Kernel.Shared.Abstractions;
using SingularityInterview.APIs.Domain.Kernel.Shared.ValueObjects;

namespace SingularityInterview.APIs.Domain.Kernel.IAM.Models;
public interface IUser : IEntity
{
    string UserName { get; }

    Email Email { get; }
    string PasswordHash { get; }
    string PasswordSalt { get; }
}