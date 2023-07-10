using Application.Dto;
using Domain.Core.Implementations;
using Infrastructure.Mapping.Orders;

namespace Infrastructure.Mapping.People;

public static class CustomerMapping
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto(
            customer.PersonId,
            customer.FullName.ToString(),
            customer.ContactInfo.Select(x => x.Contact).ToList(),
            customer.OrderHistory.Select(x => x.ToDto()).ToList());
    }
}