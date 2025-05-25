using Type = AnimeApi.Server.DataAccess.Models.Type;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface ITypeRepository : IRepository<Type>
{
    Task<IEnumerable<Type>> GetByNameAsync(string name);
    Task<IEnumerable<int>> GetExistingIdsAsync();
    Task<IEnumerable<string>> GetExistingNamesAsync();
}