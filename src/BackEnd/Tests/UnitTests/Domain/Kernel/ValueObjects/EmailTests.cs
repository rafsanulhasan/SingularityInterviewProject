using Bogus;
using FluentAssertions;
using SingularityInterview.APIs.Domain.Kernel.Shared.ValueObjects;

namespace SingularityInterview.APIs.Tests.UnitTests.Domain.Kernel.ValueObjects;

[TestFixture]
[Category("UnitTests")]
public class EmailTests
{
    private Faker _faker;

    [OneTimeSetUp]
    public void Bootstrap()
    {
        _faker = new Faker();
    }

    [Test]
    public void CreateEmailFromString_ShouldCreate_ValidEmailWithoutException()
    {
        // Arrange
        string expectedEmail = _faker.Internet.Email();

        // Act
        Email originalEmail = Email.Create(expectedEmail);

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail.Switch(optionalResult =>
            {
                optionalResult.IsFaultedOrNone.Should().BeFalse();
                optionalResult.IfSucc(oEmail => oEmail.Should().Be(expectedEmail));
            });

            originalEmail.Exception.IsNone.Should().BeTrue();
        });
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void CreateEmailFromNullString_ShouldCreate_OptionalEmailWithoutException(
        string? expectedEmail)
    {
        // Act
        Email originalEmail = Email.Create(expectedEmail);

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail
                .Switch(optionalResult =>
                {
                    optionalResult.IsFaulted.Should().BeFalse();
                    optionalResult.IsNone.Should().BeTrue();
                });
            originalEmail.Exception.IsNone.Should().BeTrue();
        });
    }

    [Test]
    [TestCase("abc")]
    [TestCase("xyz")]
    [TestCase("@xyz")]
    [TestCase("@xyz.com")]
    public void CreateEmailFromInvalidEmailString_ShouldCreate_InvalidEmailWithException(
        string? expectedEmail)
    {
        // Act
        Email originalEmail = Email.Create(expectedEmail);

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail.IsT0.Should().BeTrue();
            originalEmail
                .Switch(optionalResult =>
                {
                    optionalResult.IsNone.Should().BeFalse();
                    optionalResult.IsFaulted.Should().BeTrue();
                });

            originalEmail.Exception.IsSome.Should().BeTrue();
            originalEmail.Exception.IsNone.Should().BeFalse();
        });
    }

    [Test]
    public void CastEmailFromValidEmailString_ShouldCreate_ValidEmailWithoutException()
    {
        // Arrange
        string expectedEmail = _faker.Internet.Email();

        // Act
        Email originalEmail = expectedEmail;

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail.IsT0.Should().BeTrue();
            originalEmail.AsT0.IfSucc(oEmail => oEmail.Should().Be(expectedEmail));
            originalEmail.Exception.IsNone.Should().BeTrue();
        });
    }

    [Test]
    public void CastStringFromValidEmail_ShouldReturn_ValidEmailString()
    {
        // Arrange
        Email expectedEmail = Email.Create(
            _faker.Internet.Email());

        // Act
        string originalEmail = expectedEmail;

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail
                .Should()
                .Be(expectedEmail);

            expectedEmail.Exception.IsNone.Should().BeTrue();
        });
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void CastStringFromNullOrEmptyEmail_ShouldReturn_EmptyEmailStringWithException(string? mail)
    {
        // Arrange
        Email expectedEmail = Email.Create(mail);

        // Act
        string originalEmail = expectedEmail;

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail
                .Should()
                .BeEmpty();

            expectedEmail.Exception.IsNone.Should().BeTrue();
        });
    }

    [Test]
    [TestCase("xyz")]
    [TestCase("xyz@")]
    [TestCase("@xyz")]
    [TestCase("xyz@.com")]
    [TestCase(".xyz")]
    [TestCase("abc.xyz")]
    public void CastStringFromInvalidEmail_ShouldReturn_EmptyEmailWithException(string mail)
    {
        // Arrange
        Email expectedEmail = Email.Create(mail);

        // Act
        string originalEmail = expectedEmail;

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail
                .Should()
                .BeEmpty();

            expectedEmail.Exception.IsSome.Should().BeTrue();
        });
    }

    [Test]
    public void GetHashCodeWhenEmailIsValid_ShouldReturn_HashcodeFromEmail()
    {
        // Arrange
        string emailStr = _faker.Internet.Email();
        Email email = Email.Create(emailStr);
        int expectedHahCode = emailStr.GetHashCode();

        // Act
        int actualHashCode = email.GetHashCode();

        // Assert
        actualHashCode.Should().Be(expectedHahCode);
    }

    [Test]
    [TestCase("@.com")]
    public void GetHashCodeWhenEmailIsInvalid_ShouldReturn_HashcodeFromException(string emailStr)
    {
        // Arrange
        Email email = Email.Create(emailStr);

        // Act
        int actualHashCode = email.GetHashCode();

        // Assert
        int expectedHahCode = email.Exception.GetHashCode();
        actualHashCode.Should().Be(expectedHahCode);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("")]
    public void GetHashCodeWhenEmailIsNull_ShouldReturn_HashcodeFromEmptyString(string? emailStr)
    {
        // Arrange
        Email email = Email.Create(emailStr);

        // Act
        int actualHashCode = email.GetHashCode();

        // Assert
        int expectedHahCode = string.Empty.GetHashCode();
        actualHashCode.Should().Be(expectedHahCode);
    }

    [Test]
    public void ToStringFromValidEmail_ShouldReturn_EmailWithoutException()
    {
        // Arrange
        Email expectedEmail = Email.Create(_faker.Internet.Email());

        // Act
        string originalEmail = expectedEmail.ToString();

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail
                .Should()
                .Be((string)expectedEmail);

            expectedEmail.Exception.IsNone.Should().BeTrue();
        });
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void ToStringFromNullOrEmptyEmail_ShouldReturn_EmptyEmailWithoutException(string? mail)
    {
        // Arrange
        Email expectedEmail = Email.Create(mail);

        // Act
        string originalEmail = expectedEmail.ToString();

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail
                .Should()
                .BeEmpty();

            expectedEmail.Exception.IsNone.Should().BeTrue();
        });
    }

    [Test]
    [TestCase("xyz")]
    [TestCase("xyz@")]
    [TestCase("@xyz")]
    [TestCase("xyz@.com")]
    [TestCase(".xyz")]
    [TestCase("abc.xyz")]
    public void ToStringFromInvalidEmail_ShouldReturn_EmptyEmailWithException(string mail)
    {
        // Arrange
        Email expectedEmail = Email.Create(mail);

        // Act
        string originalEmail = expectedEmail.ToString();

        // Assert
        Assert.Multiple(() =>
        {
            originalEmail
                .Should()
                .BeEmpty();

            expectedEmail.Exception.IsSome.Should().BeTrue();
        });
    }
}