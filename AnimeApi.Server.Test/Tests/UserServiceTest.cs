using AnimeApi.Server.Business.Services;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Mappers;
using AnimeApi.Server.Core.Objects.Auth;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using Moq;

namespace AnimeApi.Server.Test.Tests;

public class UserServiceTest
{
    private Mock<IRepository<AppUser, AppUserDto>> _userRepositoryMock
        => new();
    private IMapper<AppUser, AppUserDto> _mapper
        => new AppUserMapper();
    
    private readonly UserService _service;
    private AppUser MockUser => new()
    {
        Id = 1,
        Email = string.Empty,
        Username = string.Empty,
        Picture_Url = String.Empty,
        Created_At = DateTime.UtcNow,
        Role_Id = 1,
        Role = new Role { Id = 1, Access = Constants.UserAccess.User },
    };

    public UserServiceTest()
    {
        _service = new UserService(_userRepositoryMock.Object, _mapper);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("another@test.com")]
    public async Task GetByEmail_Should_Return_Correct_User(string email)
    {
        var user = MockUser;
        user.Email = email;
        _userRepositoryMock
            .Setup(r => r.FindFirstOrDefaultAsync(It.IsAny<IQuerySpec<AppUser>>()))
            .ReturnsAsync(_mapper.MapToDto(user));

        var result = await _service.GetByEmailAsync(email);
        
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetById_Should_Return_Correct_User()
    {
        _userRepositoryMock
            .Setup(r => r.FindFirstOrDefaultAsync(It.IsAny<IQuerySpec<AppUser>>()))
            .ReturnsAsync(_mapper.MapToDto(MockUser));

        var result = await _service.GetByIdAsync(MockUser.Id);
        
        Assert.NotNull(result);
        Assert.Equal(MockUser.Id, result.Id);
    }

    [Fact]
    public async Task GetOrCreateUser_Should_Return_Existing_User()
    {
        var payload = new AuthPayload { Email = MockUser.Email };

        _userRepositoryMock
            .Setup(r => r.FindFirstOrDefaultAsync(It.IsAny<IQuerySpec<AppUser>>()))
            .ReturnsAsync(_mapper.MapToDto(MockUser));

        var result = await _service.GetOrCreateUserAsync(payload);
        
        Assert.NotNull(result);
        Assert.Equal(MockUser.Email, result.Email);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<AppUser>()), Times.Never);
    }


    [Fact]
    public async Task GetOrCreateUser_Should_Create_New_User()
    {
        var email = "new@example.com";
        var payload = new AuthPayload 
        { 
            Email = email,
            Picture = "https://example.com/picture.jpg"
        };

        _userRepositoryMock
            .Setup(r => r.FindFirstOrDefaultAsync(It.IsAny<IQuerySpec<AppUser>>()))
            .ReturnsAsync((AppUserDto?)null);

        var result = await _service.GetOrCreateUserAsync(payload);
        
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
        Assert.Equal(payload.Picture, result.ProfilePictureUrl);
        Assert.False(result.Admin);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("nonexistent@example.com", false)]
    public async Task DestroyUser_Should_Return_Correct_Result(string email, bool expectedResult)
    {
        var user = expectedResult ? new AppUser { Email = email } : null;
        
        _userRepositoryMock
            .Setup(r => r.FindFirstOrDefaultAsync(It.IsAny<IQuerySpec<AppUser>>()))
            .ReturnsAsync(_mapper.MapToDto(user));

        _userRepositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<IQuerySpec<AppUser>>()))
            .ReturnsAsync(expectedResult);

        var result = await _service.DestroyUserAsync(email);
        
        Assert.Equal(expectedResult, result);
    }
}