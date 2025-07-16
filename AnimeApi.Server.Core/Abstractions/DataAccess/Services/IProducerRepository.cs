using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface IProducerRepository : IRepository<Producer>
{
    Task<IEnumerable<Producer>> GetByNameAsync(string name);
    Task<IEnumerable<int>> GetExistingIdsAsync();
    Task<IEnumerable<string>> GetExistingNamesAsync();

}