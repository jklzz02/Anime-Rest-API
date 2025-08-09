using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public static class RefreshTokenMapper
{
    public static RefreshTokenDto ToDto(this RefreshToken refreshToken)
    {
        return new RefreshTokenDto
        {
            Id = refreshToken.Id,
            CreatedAt = refreshToken.Created_At,
            ExpiresAt = refreshToken.Expires_At,
            HashedToken = refreshToken.Hashed_Token,
            RevokedAt = refreshToken.Revoked_At,
            UserId = refreshToken.User_Id,
        };
    }

    public static RefreshToken ToModel(this RefreshTokenDto refreshToken)
    {
        return new RefreshToken
        {
            Id = refreshToken.Id,
            Created_At = refreshToken.CreatedAt,
            Expires_At = refreshToken.ExpiresAt,
            Hashed_Token = refreshToken.HashedToken,
            Revoked_At = refreshToken.RevokedAt,
            User_Id = refreshToken.UserId,
        };
    }
}