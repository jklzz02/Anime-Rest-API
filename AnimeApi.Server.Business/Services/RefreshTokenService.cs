using System.Security.Cryptography;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;
using LinqKit;

namespace AnimeApi.Server.Business.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ITokenHasher _hasher;
    private readonly IRepository<RefreshToken, RefreshTokenDto> _repository;
    
    public RefreshTokenService(
        ITokenHasher tokenHasher,
        IRepository<RefreshToken, RefreshTokenDto> repository)
    {
        _hasher = tokenHasher;
        _repository = repository;
    }

    public async Task<RefreshTokenResult> CreateAsync(int userId)
    {
        var token = GenerateToken();
        var hashed = _hasher.Hash(token);
        var refreshToken = new RefreshTokenDto
        {
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(Constants.Auth.RefreshTokenExpirationDays),
            HashedToken = hashed,
            RevokedAt = null,
            UserId = userId
        };

        var saved = await 
            _repository.AddAsync(refreshToken);

        if (saved is null)
        {
            throw new Exception("Failed to create refresh token");
        }

        return new RefreshTokenResult
        {
            Token = token,
            MetaData = saved.Data
        };
    }

    public async Task<RefreshTokenValidation> ValidateAsync(string token)
    {
        var hashed = _hasher.Hash(token);
        
        var query = new TokenQuery()
            .ByToken(hashed);
        
        var refreshToken = await 
            _repository.FindFirstOrDefaultAsync(query);

        return new RefreshTokenValidation
        {
            UserId = refreshToken?.UserId ?? 0,
            Success = refreshToken?.IsActive ?? false
        };
    }

    public async Task<bool> RevokeAsync(string token)
    {
        var dto = await
            _repository.FindFirstOrDefaultAsync(
                new TokenQuery()
                    .ByToken(token)
            );

        if (dto is null)
        {
            return false;
        }

        dto.Revoke();

        var result = await
            _repository.UpdateAsync(dto);

        return result.IsSuccess;
    }
    
    public async Task<bool> RevokeByUserIdAsync(int userId)
    {
        var query = new TokenQuery()
            .ByUser(userId);
        
        var result = await
            _repository.DeleteAsync(query);
        
        return result;
    }

    private string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}