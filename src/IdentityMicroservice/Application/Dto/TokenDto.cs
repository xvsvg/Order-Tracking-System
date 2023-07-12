namespace Application.Dto;

public readonly record struct TokenDto(string AccessToken, string RefreshToken, DateTime Expiration);