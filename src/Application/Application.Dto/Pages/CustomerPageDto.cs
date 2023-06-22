namespace Application.Dto.Pages;

public record CustomerPageDto(IEnumerable<CustomerDto> Customers, int Page, int TotalPages);