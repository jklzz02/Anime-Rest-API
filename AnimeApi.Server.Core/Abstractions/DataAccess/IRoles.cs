using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess;

public interface IRoles
{
    Task<Role> AdminAsync();
    
    Task<Role> UserAsync();
}