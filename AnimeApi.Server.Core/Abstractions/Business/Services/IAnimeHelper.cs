using System.Linq.Expressions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IAnimeHelper
{
    Dictionary<string, string> ErrorMessages { get; }
    Task<AnimeDto?> GetByIdAsync(int id);
    Task<IEnumerable<AnimeDto>> GetAllAsync();
    Task<PaginatedResult<AnimeDto>?> GetAllAsync(int page, int size = 100);
    Task<IEnumerable<AnimeDto>> GetByIdsAsync(IEnumerable<int> ids);
    Task<PaginatedResult<AnimeDto>?> GetAllNonAdultAsync(int page, int size);
    Task<PaginatedResult<AnimeDto>?> GetByNameAsync(string name, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByProducerAsync(int producerId, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByLicensorAsync(int licensorId, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByGenreAsync(int genreId, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetBySourceAsync(string source, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByEnglishNameAsync(string englishName, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByScoreAsync(int score, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByReleaseYearAsync(int year, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByEpisodesAsync(int episodes, int page, int size = 100);
    Task<PaginatedResult<AnimeDto>?> GetByTypeAsync(string type, int page, int size = 100);
    Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count);
    Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);
    Task<IEnumerable<AnimeSummaryDto>> GetSummariesAsync(int count); 
    Task<AnimeDto?> CreateAsync(AnimeDto entity);
    Task<AnimeDto?> UpdateAsync(AnimeDto entity);
    Task<bool> DeleteAsync(int id);
    Task<PaginatedResult<AnimeDto>?> SearchAsync(AnimeSearchParameters parameters, int page, int size = 100);
}