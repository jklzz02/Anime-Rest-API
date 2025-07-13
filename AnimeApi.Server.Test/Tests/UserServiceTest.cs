using AnimeApi.Server.Business;
using AnimeApi.Server.Business.Services;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Google.Apis.Auth;
using Moq;

namespace AnimeApi.Server.Test.Tests;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
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
        _userRepositoryMock = new Mock<IUserRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _service = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("another@test.com")]
    public async Task GetByEmail_Should_Return_Correct_User(string email)
    {
        var user = MockUser;
        user.Email = email;
        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(user);

        var result = await _service.GetByEmailAsync(email);
        
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetById_Should_Return_Correct_User()
    {
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(MockUser.Id))
            .ReturnsAsync(MockUser);

        var result = await _service.GetByIdAsync(MockUser.Id);
        
        Assert.NotNull(result);
        Assert.Equal(MockUser.Id, result.Id);
    }

    [Fact]
    public async Task GetOrCreateUser_Should_Return_Existing_User()
    {
        var payload = new GoogleJsonWebSignature.Payload { Email = MockUser.Email };

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(MockUser.Email))
            .ReturnsAsync(MockUser);

        var result = await _service.GetOrCreateUserAsync(payload);
        
        Assert.NotNull(result);
        Assert.Equal(MockUser.Email, result.Email);
        _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<AppUser>()), Times.Never);
    }


    [Fact]
    public async Task GetOrCreateUser_Should_Create_New_User()
    {
        var email = "new@example.com";
        var roleId = 1;
        var payload = new GoogleJsonWebSignature.Payload 
        { 
            Email = email,
            Picture = "https://example.com/picture.jpg"
        };

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync((AppUser?)null);

        _roleRepositoryMock
            .Setup(r => r.GetByAccessAsync(Constants.UserAccess.User))
            .ReturnsAsync(new Role { Id = roleId });

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
            .Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(r => r.DestroyAsync(email))
            .ReturnsAsync(expectedResult);

        var result = await _service.DestroyUserAsync(email);
        
        Assert.Equal(expectedResult, result);
    }
}