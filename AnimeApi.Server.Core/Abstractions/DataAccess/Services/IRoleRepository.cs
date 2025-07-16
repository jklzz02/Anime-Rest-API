using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface IRoleRepository
{
    Task<Role?> GetByAccessAsync(string access);
    Task<Role?> GetByIdAsync(int id);
}