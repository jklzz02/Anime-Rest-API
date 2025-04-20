using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface IGenreRepository : IRepository<Genre>
{
    Task<IEnumerable<Genre>> GetByNameAsync(string name);
}