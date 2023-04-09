using OTS.Domain.Domain.Common.Exceptions;
using OTS.Domain.Domain.Core.Contracts;
using OTS.Domain.Domain.Core.Utils;

namespace OTS.Domain.Domain.Core.ValueObjects;

public record AccountCredentials(Person Person, string Email, string Password)
{
    public Person Person { get; } = Person;
    public string Email { get; } = Validate(Email);
    public string Password { get; } = PasswordHasher.Hash(Password);

    private static string Validate(string value)
    {
        if (value.Contains('@') is false)
            throw new InvalidEmailException("Specify correct email");

        return value;
    }
}