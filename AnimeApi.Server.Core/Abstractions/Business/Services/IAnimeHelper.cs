using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Partials;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IAnimeHelper
{
    Task<AnimeDto?> GetByIdAsync(int id);
    Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids);
    Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids, string orderBy, string direction);
    Task<IEnumerable<AnimeDto>> GetAllAsync();
    Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size);
    Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size, bool includeAdult);
    Task<IEnumerable<AnimeListItem>> GetAnimeListAsync(int count);
    Task<IEnumerable<AnimeListItem>> GetAnimeListByQueryAsync(string textQuery, int count);
    Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count);
    Task<AnimeSummary?> GetSummaryByIdAsync(int id);
    Task<IEnumerable<AnimeSummary>> GetSummariesByIdAsync(IEnumerable<int> ids);
    Task<IEnumerable<AnimeSummary>> GetSummariesByIdAsync(IEnumerable<int> ids, string orderBy, string direction);
    Task<IEnumerable<AnimeSummary>> GetSummariesAsync(int count); 
    Task<Result<AnimeDto>> CreateAsync(AnimeDto entity);
    Task<Result<AnimeDto>> UpdateAsync(AnimeDto entity);
    Task<bool> DeleteAsync(int id);
    Task<PaginatedResult<AnimeDto>> SearchAsync(AnimeSearchParameters parameters, int page, int size = 100);
}