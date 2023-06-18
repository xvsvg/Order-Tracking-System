namespace Application.Dto;

public record CustomerDto(Guid Id, string Name, IEnumerable<string> ContactInfo, IEnumerable<OrderDto> History);