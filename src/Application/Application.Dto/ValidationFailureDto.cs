namespace Application.Dto;

public record ValidationFailureDto(IDictionary<string, string> Errors);