using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Objects.Partials;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.Business.Services;

/// <summary>
/// Provides user-related services including user retrieval, creation, and deletion.
/// Implements the <see cref="IUserService"/> interface.
/// </summary>
public class UserService(IRepository<AppUser, AppUserDto> userRepository) : IUserService
{
    /// <inheritdoc />
    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var query = new UserQuery().ByEmail(email);
        
        return await
            userRepository.FindFirstOrDefaultAsync(query);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<PublicUser>> GetPublicUsersAsync(int page, int pageSize)
    {
        var count =  await userRepository.CountAsync();

        var results = await
            userRepository.FindAsync<PublicUser>(new UserQuery()
                .SortByEmail()
                .TieBreaker());
        
        return new PaginatedResult<PublicUser>(results, page, pageSize, count);
    }

    /// <inheritdoc />
    public async Task<AppUserDto?> GetByIdAsync(int id)
    {
        var query = new UserQuery().ByPk(id);
        
        return await
            userRepository.FindFirstOrDefaultAsync(query);
    }

    /// <inheritdoc />
    public async Task<PublicUser?> GetPublicUserAsync(int id)
    {
        var  query = new UserQuery().ByPk(id);
        return await
            userRepository.FindFirstOrDefaultAsync<PublicUser>(query);
    }

    /// <inheritdoc />
    public async Task<AppUserDto> GetOrCreateUserAsync(AuthPayload payload)
    {
        var query = new UserQuery().ByEmail(payload.Email);
        
        var existingUser = await
            userRepository.FindFirstOrDefaultAsync(query);

        if (existingUser != null)
        {
            return existingUser;
        }

        var newUser = new AppUserDto
        {
            Username = payload.Username,
            Email = payload.Email,
            CreatedAt = DateTime.UtcNow,
            ProfilePictureUrl = payload.Picture,
            Admin = false
        };

        var result = await 
            userRepository.AddAsync(newUser);
        
        return result.Data;
    }

    /// <inheritdoc />
    public async Task<bool> DestroyUserAsync(int id)
    {
        var query =  new UserQuery().ByPk(id);
        
        return await
            userRepository.DeleteAsync(query);
    }

    /// <inheritdoc />
    public async Task<bool> DestroyUserAsync(string email)
    {
        var query = new UserQuery().ByEmail(email);
        
        return await 
            userRepository.DeleteAsync(query);
    }
}