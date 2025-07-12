using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByAccessAsync(string access);
    Task<Role?> GetByIdAsync(int id);
}