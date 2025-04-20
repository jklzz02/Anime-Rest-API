using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface ILicensorRepository : IRepository<Licensor>
{
    Task<IEnumerable<Licensor>> GetByNameAsync(string name);
}