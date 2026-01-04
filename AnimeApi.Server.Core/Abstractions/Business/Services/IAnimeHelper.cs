using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IAnimeHelper
{
    Task<AnimeDto?> GetByIdAsync(int id);
    Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids);
    Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids, string orderBy, string direction);
    Task<IEnumerable<AnimeDto>> GetAllAsync();
    Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size);
    Task<PaginatedResult<AnimeDto>> GetAllNonAdultAsync(int page, int size);
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