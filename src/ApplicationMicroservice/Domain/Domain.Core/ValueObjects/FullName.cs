namespace Domain.Core.ValueObjects;

public sealed record FullName(string FirstName, string MiddleName, string LastName)
{
    public override string ToString()
    {
        return $"{FirstName} {MiddleName} {LastName}";
    }
}