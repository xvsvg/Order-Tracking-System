namespace Application.Dto.Pages;

public record CourierPageDto(IEnumerable<CourierDto?> Couriers, int Page, int TotalPages);