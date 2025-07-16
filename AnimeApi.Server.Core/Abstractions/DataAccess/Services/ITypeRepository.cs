using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface ITypeRepository : IRepository<Type>
{
    Task<IEnumerable<Type>> GetByNameAsync(string name);
    Task<IEnumerable<int>> GetExistingIdsAsync();
    Task<IEnumerable<string>> GetExistingNamesAsync();
}