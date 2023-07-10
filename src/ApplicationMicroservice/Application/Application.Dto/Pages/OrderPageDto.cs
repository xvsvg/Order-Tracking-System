namespace Application.Dto.Pages;

public record OrderPageDto(IEnumerable<OrderDto?> Orders, int Page, int TotalPages);