using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Mappers;

public class RefreshTokenMapper : Mapper<RefreshToken, RefreshTokenDto>
{
    public override RefreshTokenDto MapToDto(RefreshToken refreshToken)
    {
        return new RefreshTokenDto
        {
            Id = refreshToken.Id,
            CreatedAt = refreshToken.CreatedAt,
            ExpiresAt = refreshToken.ExpiresAt,
            HashedToken = refreshToken.HashedToken,
            RevokedAt = refreshToken.RevokedAt,
            UserId = refreshToken.UserId,
        };
    }

    public override RefreshToken MapToEntity(RefreshTokenDto refreshToken)
    {
        return new RefreshToken
        {
            Id = refreshToken.Id,
            CreatedAt = refreshToken.CreatedAt.ToUniversalTime(),
            ExpiresAt = refreshToken.ExpiresAt.ToUniversalTime(),
            HashedToken = refreshToken.HashedToken,
            RevokedAt = refreshToken.RevokedAt?.ToUniversalTime(),
            UserId = refreshToken.UserId,
        };
    }
}