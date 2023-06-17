namespace Domain.Common.Exceptions;

public class InvalidFullNameException : DomainException
{
    public InvalidFullNameException(string? message)
        : base(message) { }
}