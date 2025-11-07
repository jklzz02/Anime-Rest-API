using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;
using LinqKit;

namespace AnimeApi.Server.Business.Services.Helpers;

public class RefreshTokenHelper : IRefreshTokenHelper
{
    private readonly IRepository<RefreshToken, RefreshTokenDto> _repository;
    private readonly IMapper<RefreshToken, RefreshTokenDto> _mapper;
    
    public RefreshTokenHelper(
        IRepository<RefreshToken, RefreshTokenDto> repository,
        IMapper<RefreshToken, RefreshTokenDto> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<RefreshTokenDto?> GetByIdAsync(int id)
    {
        var query = new TokenQuery().ByPk(id);

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<RefreshTokenDto?> GetByTokenAsync(string token)
    {
        var query = new TokenQuery().ByToken(token);

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }
    public async Task<RefreshTokenDto?> GetByUserIdAsync(int userId)
    {
        var query = new TokenQuery().ByUser(userId);

        return await 
            _repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<RefreshTokenDto?> AddAsync(RefreshTokenDto refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));
        
        var result = await _repository.AddAsync(refreshToken);
        return result.Data;
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
        var query = new TokenQuery().ByUser(userId);

        
        var result = await
            _repository.DeleteAsync(query);
        
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new TokenQuery().ByPk(id);

        return await
            _repository.DeleteAsync(query);
    }

    public async Task<bool> DeleteByUserIdAsync(int userId)
    {
        var query = new TokenQuery().ByUser(userId);

        return await
            _repository.DeleteAsync(query);
    }
}