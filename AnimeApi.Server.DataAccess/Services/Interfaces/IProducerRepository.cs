using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface IProducerRepository : IRepository<Producer>
{
    Task<IEnumerable<Producer>> GetByNameAsync(string name);
}