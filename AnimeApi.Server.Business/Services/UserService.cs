using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Auth;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.Business.Services;

/// <summary>
/// Provides user-related services including user retrieval, creation, and deletion.
/// Implements the <see cref="IUserService"/> interface.
/// </summary>
public class UserService : IUserService
{
    private readonly IRepository<AppUser, AppUserDto> _userRepository;
    private readonly IMapper<AppUser, AppUserDto> _mapper;
    
    public UserService(
        IRepository<AppUser, AppUserDto> userRepository,
        IMapper<AppUser, AppUserDto> mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var query = new UserQuery().ByEmail(email);
        
        return await 
            _userRepository.FindFirstOrDefaultAsync(query);
    }

    /// <inheritdoc />
    public async Task<AppUserDto?> GetByIdAsync(int id)
    {
        var query = new UserQuery().ByPk(id);
        
        return await 
            _userRepository.FindFirstOrDefaultAsync(query);
    }

    /// <inheritdoc />
    public async Task<AppUserDto> GetOrCreateUserAsync(AuthPayload payload)
    {
        var query = new UserQuery().ByEmail(payload.Email);
        
        var existingUser = 
            await _userRepository.FindFirstOrDefaultAsync(query);

        if (existingUser != null)
        {
            return existingUser;
        }

        var newUser = new AppUserDto
        {
            Username = payload.Username,
            Email = payload.Email,
            CreatedAt = DateTime.UtcNow,
            ProfilePictureUrl = payload.Picture ?? string.Empty,
            Admin = false
        };

        var result = await 
            _userRepository.AddAsync(newUser);
        
        return result.Data;
    }

    /// <inheritdoc />
    public async Task<bool> DestroyUserAsync(string email)
    {
        var query = new UserQuery().ByEmail(email);
        
        return await 
            _userRepository.DeleteAsync(query);
    }
}