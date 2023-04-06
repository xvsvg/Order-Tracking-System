using OTS.Domain.Domain.Common.Exceptions;

namespace OTS.Domain.Domain.Core.ValueObjects;

public sealed record FullName(string FirstName, string MiddleName, string LastName)
{
    public string FirstName { get; } = Validate(FirstName);
    public string MiddleName { get; } = Validate(MiddleName);
    public string LastName { get; } = Validate(LastName);

    private static string Validate(string value)
    {
        if (string.IsNullOrEmpty(value) || !char.IsUpper(value.First()))
            throw new InvalidFullNameException("Full name should start with upper case character");
        
        return value;
    }
}