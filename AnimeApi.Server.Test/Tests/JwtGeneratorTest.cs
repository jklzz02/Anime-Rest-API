using System.Security.Cryptography;
using AnimeApi.Server.Business.Services;
using AnimeApi.Server.Core.Exceptions;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AnimeApi.Server.Test.Tests;

public class JwtGeneratorTest
{
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Mock<IConfigurationSection> _jwtSectionMock = new();
    private readonly Mock<IConfigurationSection> _secretSectionMock = new();
    private readonly Mock<IConfigurationSection> _issuerSectionMock = new();
    private readonly Mock<IConfigurationSection> _audienceSectionMock = new();

    private AppUserDto DummyUser => new()
    {
        Id = 0,
        Email = string.Empty,
        Username = string.Empty,
        ProfilePictureUrl = string.Empty,
        CreatedAt = DateTime.UtcNow,
        Admin = false
    };

    private readonly JwtGenerator _generator;

    public JwtGeneratorTest()
    {
        _configMock
            .Setup(x => x.GetSection("Authentication:Jwt")).Returns(_jwtSectionMock.Object);
        
        _secretSectionMock
            .Setup(x => x.Value).Returns(GenerateRandomSecret());
        
        _generator = new JwtGenerator(_configMock.Object);
    }

    [Fact]
    public void GenerateToken_Should_Generate_Valid_Token()
    {
        var user = new AppUserDto
        {
            Id = 1,
            Email = "test@test.com",
            Username = "test",
            ProfilePictureUrl = string.Empty,
            CreatedAt = DateTime.UtcNow,
            Admin = true
        };

        _issuerSectionMock.Setup(x => x.Value).Returns("issuer");
        _audienceSectionMock.Setup(x => x.Value).Returns("audience");

        _jwtSectionMock.Setup(x => x.GetSection("Secret")).Returns(_secretSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Issuer")).Returns(_issuerSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Audience")).Returns(_audienceSectionMock.Object);

        var token = _generator.GenerateToken(user);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void GenerateToken_Should_Throw_When_Secret_Missing()
    {
        _secretSectionMock.Setup(x => x.Value).Returns(string.Empty);
        _issuerSectionMock.Setup(x => x.Value).Returns("issuer");
        _audienceSectionMock.Setup(x => x.Value).Returns("audience");

        _jwtSectionMock.Setup(x => x.GetSection("Secret")).Returns(_secretSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Issuer")).Returns(_issuerSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Audience")).Returns(_audienceSectionMock.Object);

        Assert.Throws<ConfigurationException>(() => _generator.GenerateToken(DummyUser));
    }

    [Fact]
    public void GenerateToken_Should_Throw_When_Issuer_Missing()
    {
        _issuerSectionMock.Setup(x => x.Value).Returns(string.Empty);
        _audienceSectionMock.Setup(x => x.Value).Returns("audience");

        _jwtSectionMock.Setup(x => x.GetSection("Secret")).Returns(_secretSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Issuer")).Returns(_issuerSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Audience")).Returns(_audienceSectionMock.Object);

        Assert.Throws<ConfigurationException>(() => _generator.GenerateToken(DummyUser));
    }

    [Fact]
    public void GenerateToken_Should_Throw_When_Audience_Missing()
    {
        _issuerSectionMock.Setup(x => x.Value).Returns("issuer");
        _audienceSectionMock.Setup(x => x.Value).Returns(string.Empty);

        _jwtSectionMock.Setup(x => x.GetSection("Secret")).Returns(_secretSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Issuer")).Returns(_issuerSectionMock.Object);
        _jwtSectionMock.Setup(x => x.GetSection("Audience")).Returns(_audienceSectionMock.Object);

        Assert.Throws<ConfigurationException>(() => _generator.GenerateToken(DummyUser));
    }
    
    private static string GenerateRandomSecret(int byteLength = 32)
    {
        var bytes = new byte[byteLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
