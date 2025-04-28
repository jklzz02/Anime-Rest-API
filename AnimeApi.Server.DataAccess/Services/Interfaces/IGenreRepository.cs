using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface IGenreRepository : IRepository<Genre>
{
    Task<IEnumerable<Genre>> GetByNameAsync(string name);
}