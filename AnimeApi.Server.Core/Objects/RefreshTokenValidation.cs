namespace AnimeApi.Server.Core.Objects;

public record RefreshTokenValidation
{
    public int UserId { get; init; }
    public bool Success { get; init; }
}