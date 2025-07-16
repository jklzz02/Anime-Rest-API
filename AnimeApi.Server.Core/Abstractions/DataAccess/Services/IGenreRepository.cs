using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface IGenreRepository : IRepository<Genre>
{
    Task<IEnumerable<Genre>> GetByNameAsync(string name);
    Task<IEnumerable<int>> GetExistingIdsAsync();
    Task<IEnumerable<string>> GetExistingNamesAsync();
}