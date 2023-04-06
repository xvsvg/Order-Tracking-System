﻿namespace OTS.Domain.Domain.Common.Exceptions;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string message)
        : base(message) { }
}