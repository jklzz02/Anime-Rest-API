using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Business.Services.Helpers;

public class RefreshTokenHelper : IRefreshTokenHelper
{
    private readonly IRefreshTokenRepository _repository;
    
    public RefreshTokenHelper(IRefreshTokenRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<RefreshTokenDto?> GetByIdAsync(int id)
    {
        var refreshToken = await _repository.GetByIdAsync(id);
        return refreshToken?.ToDto();
    }

    public async Task<RefreshTokenDto?> GetByTokenAsync(string token)
    {
        var refreshToken = await _repository.GetByTokenAsync(token);
        return refreshToken?.ToDto();
    }

    public async Task<RefreshTokenDto?> GetByUserIdAsync(int userId)
    {
        var refreshToken = await _repository.GetByUserIdAsync(userId);
        return refreshToken?.ToDto();
    }

    public async Task<RefreshTokenDto?> AddAsync(RefreshTokenDto refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));
        
        var model = refreshToken.ToModel();
        var result = await _repository.AddAsync(model);
        return result?.ToDto();
    }

    public async Task<bool> RevokeAsync(string token)
    {
        return await _repository.RevokeAsync(token);
    }

    public async Task<bool> RevokeByUserIdAsync(int userId)
    {
        return await _repository.RevokeByUserIdAsync(userId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<bool> DeleteByUserIdAsync(int userId)
    {
        return await _repository.DeleteByUserIdAsync(userId);
    }
}