using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Mappers;

public class RefreshTokenMapper : Mapper<RefreshToken, RefreshTokenDto>, IMapper<RefreshToken, RefreshTokenDto>
{
    public override RefreshTokenDto MapToDto(RefreshToken refreshToken)
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

    public override RefreshToken MapToEntity(RefreshTokenDto refreshToken)
    {
        return new RefreshToken
        {
            Id = refreshToken.Id,
            Created_At = refreshToken.CreatedAt.ToUniversalTime(),
            Expires_At = refreshToken.ExpiresAt.ToUniversalTime(),
            Hashed_Token = refreshToken.HashedToken,
            Revoked_At = refreshToken.RevokedAt?.ToUniversalTime(),
            User_Id = refreshToken.UserId,
        };
    }
}