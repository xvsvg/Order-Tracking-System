namespace Application.Dto;

public record CourierDto(Guid Id, string Name, IEnumerable<string> ContactInfo, IEnumerable<OrderDto> DeliveryList);