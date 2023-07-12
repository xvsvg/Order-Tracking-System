namespace Application.Dto;

public readonly record struct UserDto(Guid Id, string Username, string? RefreshToken, DateTime TokenExpiration);