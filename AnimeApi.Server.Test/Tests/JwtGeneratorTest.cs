using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AnimeApi.Server.Business.Services;
using AnimeApi.Server.Core.Exceptions;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AnimeApi.Server.Test.Tests;

public class JwtGeneratorTest
{
    private const string ValidSecret = "ThisIsAVerySecureSecretKeyForTestingPurposesOnly123!";
    private const string ValidIssuer = "TestIssuer";
    private const string ValidAudience = "TestAudience";

    private static AppUserDto CreateTestUser() => new()
    {
        Id = 1,
        Email = "test@test.com",
        Username = "testuser",
        ProfilePictureUrl = string.Empty,
        CreatedAt = DateTime.UtcNow,
        Admin = true
    };

    private static Mock<IConfiguration> CreateConfigurationMock(
        string? secret = ValidSecret,
        string? issuer = ValidIssuer,
        string? audience = ValidAudience)
    {
        var configMock = new Mock<IConfiguration>();

        var jwtSectionMock = new Mock<IConfigurationSection>();
        configMock.Setup(x => x.GetSection("Authentication:Jwt")).Returns(jwtSectionMock.Object);

        var secretSectionMock = new Mock<IConfigurationSection>();
        secretSectionMock.Setup(x => x.Value).Returns(secret);
        secretSectionMock.Setup(x => x.Path).Returns("Authentication:Jwt:Secret");
        secretSectionMock.Setup(x => x.GetChildren()).Returns(secret != null ? new[] { secretSectionMock.Object } : Enumerable.Empty<IConfigurationSection>());
        jwtSectionMock.Setup(x => x.GetSection("Secret")).Returns(secretSectionMock.Object);
        configMock.Setup(x => x.GetSection("Authentication:Jwt:Secret")).Returns(secretSectionMock.Object);

        var issuerSectionMock = new Mock<IConfigurationSection>();
        issuerSectionMock.Setup(x => x.Value).Returns(issuer);
        issuerSectionMock.Setup(x => x.Path).Returns("Authentication:Jwt:Issuer");
        issuerSectionMock.Setup(x => x.GetChildren()).Returns(issuer != null ? new[] { issuerSectionMock.Object } : Enumerable.Empty<IConfigurationSection>());
        jwtSectionMock.Setup(x => x.GetSection("Issuer")).Returns(issuerSectionMock.Object);
        configMock.Setup(x => x.GetSection("Authentication:Jwt:Issuer")).Returns(issuerSectionMock.Object);

        var audienceSectionMock = new Mock<IConfigurationSection>();
        audienceSectionMock.Setup(x => x.Value).Returns(audience);
        audienceSectionMock.Setup(x => x.Path).Returns("Authentication:Jwt:Audience");
        audienceSectionMock.Setup(x => x.GetChildren()).Returns(audience != null ? new[] { audienceSectionMock.Object } : Enumerable.Empty<IConfigurationSection>());
        jwtSectionMock.Setup(x => x.GetSection("Audience")).Returns(audienceSectionMock.Object);
        configMock.Setup(x => x.GetSection("Authentication:Jwt:Audience")).Returns(audienceSectionMock.Object);

        return configMock;
    }

    [Fact]
    public void GenerateToken_Should_Generate_Valid_Token()
    {
        var configMock = CreateConfigurationMock();
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();

        var token = generator.GenerateToken(user);

        Assert.NotNull(token);
        Assert.NotEmpty(token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        Assert.Equal(user.Id.ToString(), jwtToken.Subject);
        Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
        Assert.Equal(user.Username, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Name).Value);
        Assert.Equal(ValidIssuer, jwtToken.Issuer);
        Assert.Contains(ValidAudience, jwtToken.Audiences);
    }

    [Fact]
    public void GenerateToken_Should_Include_Admin_Role_When_User_Is_Admin()
    {
        var configMock = CreateConfigurationMock();
        var generator = new JwtGenerator(configMock.Object);
        var adminUser = CreateTestUser() with { Admin = true };

        var token = generator.GenerateToken(adminUser);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var roleClaim = jwtToken.Claims.First(c => c.Type == ClaimTypes.Role);
        
        Assert.Equal("Admin", roleClaim.Value);
    }

    [Fact]
    public void GenerateToken_Should_Include_User_Role_When_User_Is_Not_Admin()
    {
        var configMock = CreateConfigurationMock();
        var generator = new JwtGenerator(configMock.Object);
        var regularUser = CreateTestUser() with { Admin = false };

        var token = generator.GenerateToken(regularUser);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var roleClaim = jwtToken.Claims.First(c => c.Type == ClaimTypes.Role);
        
        Assert.Equal("User", roleClaim.Value);
    }

    [Fact]
    public void GenerateToken_Should_Throw_ConfigurationException_When_Secret_Missing()
    {
        var configMock = CreateConfigurationMock(secret: null);
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();

        var exception = Assert.Throws<ConfigurationException>(() => generator.GenerateToken(user));
        Assert.Equal("Authentication:Jwt:Secret", exception.ConfigurationKey);
    }

    [Fact]
    public void GenerateToken_Should_Throw_ConfigurationException_When_Secret_Empty()
    {
        var configMock = CreateConfigurationMock(secret: string.Empty);
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();

        var exception = Assert.Throws<ConfigurationException>(() => generator.GenerateToken(user));
        Assert.Equal("Authentication:Jwt:Secret", exception.ConfigurationKey);
    }

    [Fact]
    public void GenerateToken_Should_Throw_ConfigurationException_When_Issuer_Missing()
    {
        var configMock = CreateConfigurationMock(issuer: null);
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();

        var exception = Assert.Throws<ConfigurationException>(() => generator.GenerateToken(user));
        Assert.Equal("Authentication:Jwt:Issuer", exception.ConfigurationKey);
    }

    [Fact]
    public void GenerateToken_Should_Throw_ConfigurationException_When_Issuer_Empty()
    {
        var configMock = CreateConfigurationMock(issuer: string.Empty);
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();

        var exception = Assert.Throws<ConfigurationException>(() => generator.GenerateToken(user));
        Assert.Equal("Authentication:Jwt:Issuer", exception.ConfigurationKey);
    }

    [Fact]
    public void GenerateToken_Should_Throw_ConfigurationException_When_Audience_Missing()
    {
        var configMock = CreateConfigurationMock(audience: null);
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();

        var exception = Assert.Throws<ConfigurationException>(() => generator.GenerateToken(user));
        Assert.Equal("Authentication:Jwt:Audience", exception.ConfigurationKey);
    }

    [Fact]
    public void GenerateToken_Should_Throw_ConfigurationException_When_Audience_Empty()
    {
        var configMock = CreateConfigurationMock(audience: string.Empty);
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();

        var exception = Assert.Throws<ConfigurationException>(() => generator.GenerateToken(user));
        Assert.Equal("Authentication:Jwt:Audience", exception.ConfigurationKey);
    }

    [Fact]
    public void GenerateToken_Should_Set_Token_Expiration()
    {
        var configMock = CreateConfigurationMock();
        var generator = new JwtGenerator(configMock.Object);
        var user = CreateTestUser();
        var beforeGeneration = DateTime.UtcNow;

        var token = generator.GenerateToken(user);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var afterGeneration = DateTime.UtcNow;

        Assert.NotNull(jwtToken.ValidTo);
        Assert.True(jwtToken.ValidTo >= beforeGeneration.AddHours(2).AddMinutes(-1));
        Assert.True(jwtToken.ValidTo <= afterGeneration.AddHours(2).AddMinutes(1));
    }
}