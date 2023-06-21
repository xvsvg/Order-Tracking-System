namespace Application.Dto;

public record OrderDto(Guid Id, Guid CustomerId, DateTime DispatchDate, DateTime? DeliveryDate, Guid? CourierId);