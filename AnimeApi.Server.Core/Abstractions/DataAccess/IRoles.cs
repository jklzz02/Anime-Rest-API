using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess;

/// <summary>
/// Represents an abstraction for managing roles within the system.
/// </summary>
public interface IRoles
{
    /// <summary>
    /// Retrieves the role with administrator-level access permissions from the data source.
    /// </summary>
    /// <returns>
    /// A <see cref="Role"/> object representing the administrator role.
    /// </returns>
    Task<Role> AdminAsync();

    /// <summary>
    /// Retrieves the role with user-level access permissions from the data source.
    /// </summary>
    /// <returns>
    /// A <see cref="Role"/> object representing the user role.
    /// </returns>
    Task<Role> UserAsync();
}