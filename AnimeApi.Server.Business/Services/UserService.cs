using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;
using Google.Apis.Auth;

namespace AnimeApi.Server.Business.Services;

/// <summary>
/// Provides user-related services including user retrieval, creation, and deletion.
/// Implements the <see cref="IUserService"/> interface.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">The repository for managing user data.</param>
    /// <param name="roleRepository">The repository for managing role data.</param>
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    /// <inheritdoc />
    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var user =  await _userRepository.GetByEmailAsync(email);
        return user?.ToDto();
    }

    /// <inheritdoc />
    public async Task<AppUserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToDto();
    }

    /// <inheritdoc />
    public async Task<AppUserDto> GetOrCreateUserAsync(GoogleJsonWebSignature.Payload payload)
    {
        var existingUser = await _userRepository.GetByEmailAsync(payload.Email);

        if (existingUser != null)
        {
            return existingUser.ToDto();
        }

        var newUser = new AppUserDto
        {
            Username = payload.Email.EmailToUsername(),
            Email = payload.Email,
            CreatedAt = DateTime.UtcNow,
            ProfilePictureUrl = payload.Picture ?? string.Empty,
            Admin = false
        };

        var role = await _roleRepository
            .GetByAccessAsync(Constant.UserAccess.User);

        await _userRepository.CreateAsync(newUser.ToModel(role!.Id));
        return newUser;
    }

    /// <inheritdoc />
    public async Task<bool> DestroyUserAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        return await _userRepository.DestroyAsync(email);
    }
}