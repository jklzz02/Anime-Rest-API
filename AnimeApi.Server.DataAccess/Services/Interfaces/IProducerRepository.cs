using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface IProducerRepository : IRepository<Producer>
{
    Task<IEnumerable<Producer>> GetByNameAsync(string name);
}