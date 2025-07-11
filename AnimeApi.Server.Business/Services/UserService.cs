using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Google.Apis.Auth;

namespace AnimeApi.Server.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var user =  await _userRepository.GetByEmailAsync(email);
        return user?.ToDto();
    }

    public async Task<AppUserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToDto();
    }

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
            .GetByAccessAsync(Constants.UserAccess.User);

        await _userRepository.CreateAsync(newUser.ToModel(role!.Id));
        return newUser;
    }

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