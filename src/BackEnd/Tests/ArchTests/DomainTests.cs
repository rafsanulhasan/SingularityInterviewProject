using NetArchTest.Rules;
using SingularityInterview.APIs.Domain.Kernel.IAM.Models;
using SingularityInterview.APIs.Domain.Kernel.Shared.Exceptions;
using SingularityInterview.APIs.Domain.Kernel.Shared.ValueObjects;

namespace SingularityInterview.APIs.Tests.ArchTests;

[TestFixture]
public class DomainTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ValueObjects_ShouldHave_DependencyOn_DomainExceptions()
    {
        Type type = typeof(Email);
        Types
            .InAssembly(type.Assembly)
            .That()
            .ResideInNamespace(type.Namespace)
            .Should()
            .HaveDependencyOnAny(typeof(InvalidEmailException).Namespace)
            .GetResult();
    }

    [Test]
    public void DomainAbstractions_ShouldHave_DependencyOn_ValueObjects()
    {
        Type type = typeof(IUser);
        Types
            .InAssembly(type.Assembly)
            .That()
            .ResideInNamespace(type.Namespace)
            .Should()
            .HaveDependencyOnAny(typeof(Email).Namespace, typeof(InvalidEmailException).Namespace)
            .GetResult();
    }
}