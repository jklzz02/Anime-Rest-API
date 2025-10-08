using System.Linq.Expressions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IAnimeHelper
{
    Task<AnimeDto?> GetByIdAsync(int id);
    Task<IEnumerable<AnimeDto>> GetAllAsync();
    Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size = 100);
    Task<IEnumerable<AnimeDto>> GetByIdsAsync(IEnumerable<int> ids);
    Task<PaginatedResult<AnimeDto>> GetAllNonAdultAsync(int page, int size);
    Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count);
    Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);
    Task<IEnumerable<AnimeSummaryDto>> GetSummariesAsync(int count); 
    Task<Result<AnimeDto>> CreateAsync(AnimeDto entity);
    Task<Result<AnimeDto>> UpdateAsync(AnimeDto entity);
    Task<bool> DeleteAsync(int id);
    Task<PaginatedResult<AnimeDto>> SearchAsync(AnimeSearchParameters parameters, int page, int size = 100);
}