namespace Application.Dto;

public record OrderDto(Guid Id, string Name, Guid CustomerId, Guid? CourierId, DateTime DispatchDate, DateTime? DeliveryDate);