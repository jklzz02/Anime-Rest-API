using System.Security.Cryptography;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Business.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ITokenHasher _hasher;
    private readonly IRefreshTokenHelper _helper;
    
    public RefreshTokenService(
        ITokenHasher tokenHasher,
        IRefreshTokenHelper refreshTokenHelper)
    {
        _hasher = tokenHasher;
        _helper = refreshTokenHelper;
    }

    public async Task<RefreshTokenResult> CreateAsync(int userId)
    {
        var token = GenerateToken();
        var hashed = _hasher.Hash(token);
        var refreshToken = new RefreshTokenDto
        {
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(Constants.Authentication.RefreshTokenExpirationDays),
            HashedToken = hashed,
            RevokedAt = null,
            UserId = userId
        };

        var saved = await _helper.AddAsync(refreshToken);

        if (saved is null)
        {
            throw new ApplicationException("Failed to create refresh token");
        }

        return new RefreshTokenResult
        {
            Token = token,
            MetaData = saved
        };
    }

    public async Task<RefreshTokenValidation> ValidateAsync(string token)
    {
        var hashed = _hasher.Hash(token);
        var refreshToken = await _helper.GetByTokenAsync(hashed);

        return new RefreshTokenValidation
        {
            UserId = refreshToken?.UserId ?? 0,
            Success = refreshToken?.IsActive ?? false
        };
    }

    public async Task<bool> RevokeAsync(string token)
    {
        var hashed = _hasher.Hash(token);
        return await _helper.RevokeAsync(hashed);
    }
    
    public async Task<bool> RevokeByUserIdAsync(int userId)
    {
        return await _helper.RevokeByUserIdAsync(userId);
    }

    private string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}