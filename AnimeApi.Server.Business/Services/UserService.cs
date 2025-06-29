using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Google.Apis.Auth;

namespace AnimeApi.Server.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var user =  await _repository.GetByEmailAsync(email);
        return user?.ToDto();
    }
    
    public async Task<AppUserDto> GetOrCreateUserAsync(GoogleJsonWebSignature.Payload payload)
    {
        var existingUser = await _repository.GetByEmailAsync(payload.Email);

        if (existingUser != null)
        {
            return existingUser.ToDto();
        }

        var newUser = new AppUserDto
        {
            Username = payload.Email.EmailToUsername(),
            Email = payload.Email,
            CreatedAt = DateTime.UtcNow,
            ProfilePictureUrl = payload.Picture,
        };

        await _repository.CreateAsync(newUser.ToModel());
        return newUser;
    }
}