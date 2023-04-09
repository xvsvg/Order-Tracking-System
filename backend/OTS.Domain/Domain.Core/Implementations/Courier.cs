using OTS.Domain.Domain.Core.Contracts;
using OTS.Domain.Domain.Core.ValueObjects;

namespace OTS.Domain.Domain.Core.Implementations;

public class Courier : Person
{
    protected Courier() { }

    public Courier(FullName fullName, params ContactInfo[] contactInfo)
        : base(fullName, contactInfo) { }
}