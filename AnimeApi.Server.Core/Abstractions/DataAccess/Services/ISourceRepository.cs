using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface ISourceRepository : IRepository<Source>
{
    Task<IEnumerable<Source>> GetByNameAsync(string name);
    Task<IEnumerable<int>> GetExistingIdsAsync();
    Task<IEnumerable<string>> GetExistingNamesAsync();
}