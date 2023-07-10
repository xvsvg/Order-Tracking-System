using Application.Dto;
using Domain.Core.Implementations;
using Infrastructure.Mapping.Orders;

namespace Infrastructure.Mapping.People;

public static class CourierMapping
{
    public static CourierDto ToDto(this Courier courier)
    {
        return new CourierDto(
            courier.PersonId,
            courier.FullName.ToString(),
            courier.ContactInfo.Select(x => x.Contact).ToList(),
            courier.DeliveryList.Select(x => x?.ToDto()).ToList());
    }
}