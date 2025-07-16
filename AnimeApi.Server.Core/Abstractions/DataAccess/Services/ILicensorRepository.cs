using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface ILicensorRepository : IRepository<Licensor>
{
    Task<IEnumerable<Licensor>> GetByNameAsync(string name);
    Task<IEnumerable<int>> GetExistingIdsAsync();
    Task<IEnumerable<string>> GetExistingNamesAsync();
}