namespace Application.Dto;

public record OrderDto(Guid Id, CustomerDto Customer, DateTime DispatchDate, DateTime? DeliveryDate, CourierDto? Courier);