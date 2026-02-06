using System.Security.Cryptography;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Specification;

namespace AnimeApi.Server.Business.Services;

public class RefreshTokenService(
    IUserService userService,
    ITokenHasher tokenHasher,
    IRepository<RefreshToken, RefreshTokenDto> repository)
    : IRefreshTokenService
{
    public async Task<RefreshTokenResult> CreateAsync(int userId)
    {
        var token = GenerateToken();
        var hashed = tokenHasher.Hash(token);
        var refreshToken = new RefreshTokenDto
        {
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(Constants.Auth.RefreshTokenExpirationDays),
            HashedToken = hashed,
            RevokedAt = null,
            UserId = userId
        };

        var saved = await 
            repository.AddAsync(refreshToken);

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
        var hashed = tokenHasher.Hash(token);
        
        var query = new TokenQuery()
            .ByToken(hashed);
        
        var refreshToken = await 
            repository.FindFirstOrDefaultAsync(query);

        return new RefreshTokenValidation
        {
            UserId = refreshToken?.UserId ?? 0,
            Success = refreshToken?.IsActive ?? false
        };
    }

    public async Task<bool> RevokeAsync(string token)
    {
        var dto = await
            repository.FindFirstOrDefaultAsync(
                new TokenQuery()
                    .ByToken(token)
            );

        if (dto is null)
        {
            return false;
        }

        dto.Revoke();

        var result = await
            repository.UpdateAsync(dto);

        return result.IsSuccess;
    }
    
    public async Task<bool> RevokeByUserIdAsync(int userId)
    {
        var query = new TokenQuery()
            .ByUser(userId);
        
        var result = await
            repository.DeleteAsync(query);
        
        return result;
    }

    public async Task<bool> RevokeByEmailAsync(string email)
    {
        var linkedUsers = await userService.GetUsersLinkedToEmail(email);
        
        var query = new TokenQuery()
            .ByUser(linkedUsers.Select(u => u.Id));
        
        return await
            repository.DeleteAsync(query);
    }

    private string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}