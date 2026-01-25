using AnimeApi.Server.Core.Abstractions.Dto;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IAnimeHelper
{
    Task<AnimeDto?> GetByIdAsync(int id);
    Task<TProjection?> GetByIdAsync<TProjection>(int id) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    
    Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids);
    Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids, string orderBy, string direction);
    Task<IEnumerable<TProjection>> GetByIdAsync<TProjection>(IEnumerable<int> ids) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    Task<IEnumerable<TProjection>> GetByIdAsync<TProjection>(IEnumerable<int> ids, string orderBy, string direction) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    
    Task<IEnumerable<AnimeDto>> GetAllAsync();
    Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size);
    Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size, bool includeAdult);
    Task<PaginatedResult<TProjection>> GetAllAsync<TProjection>(int page, int size, bool includeAdult = false) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    
    Task<IEnumerable<AnimeDto>> GetAsync(int count);
    Task<IEnumerable<TProjection>> GetAsync<TProjection>(int count) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    
    Task<IEnumerable<AnimeDto>> GetByQueryAsync(string textQuery, int count);
    Task<IEnumerable<TProjection>> GetByQueryAsync<TProjection>(string textQuery, int count) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    
    Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count);
    Task<IEnumerable<TProjection>> GetMostRecentAsync<TProjection>(int count) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    
    Task<PaginatedResult<AnimeDto>> SearchAsync(AnimeSearchParameters parameters, int page, int size = 100);
    Task<PaginatedResult<TProjection>> SearchAsync<TProjection>(AnimeSearchParameters parameters, int page, int size = 100) 
        where TProjection : class, IProjectableFrom<AnimeDto>, new();
    
    Task<Result<AnimeDto>> CreateAsync(AnimeDto entity);
    Task<Result<AnimeDto>> UpdateAsync(AnimeDto entity);
    Task<bool> DeleteAsync(int id);
}