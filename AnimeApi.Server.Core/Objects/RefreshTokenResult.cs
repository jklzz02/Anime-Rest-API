using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Objects;

public class RefreshTokenResult
{
    public string Token { get; init; } = string.Empty;
    public RefreshTokenDto MetaData { get; init; } = null!;
}