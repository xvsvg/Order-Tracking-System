using Domain.Core.Contracts;
using Domain.Core.ValueObjects;

namespace Domain.Core.Implementations;

public class Courier : Person
{
    protected Courier()
    {
    }

    public Courier(Customer? customer, FullName fullName, params ContactInfo[] contactInfo)
        : base(fullName, contactInfo)
    {
        ArgumentNullException.ThrowIfNull(customer);

        Customer = customer;
    }

    public Customer? Customer { get; set; }
}