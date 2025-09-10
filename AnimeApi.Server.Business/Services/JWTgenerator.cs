using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Exceptions;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static AnimeApi.Server.Core.Constants;

namespace AnimeApi.Server.Business.Services;

/// <inheritdoc/>
public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtGenerator"/> class.
    /// </summary>
    /// <param name="configuration">The configuration containing JWT settings.</param>
    public JwtGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <inheritdoc/>
    public string GenerateToken(AppUserDto user)
    {
        var secrets = _configuration.GetSection("Authentication:Jwt");

        var secret = secrets.GetSection("Secret").Value;
        if (string.IsNullOrEmpty(secret))
            throw new ConfigurationException("Authentication:Jwt:Secret");

        var issuer = secrets.GetSection("Issuer").Value;
        if (string.IsNullOrEmpty(issuer))
            throw new ConfigurationException("Authentication:Jwt:Issuer'");

        var audience = secrets.GetSection("Audience").Value;
        if (string.IsNullOrEmpty(audience))
            throw new ConfigurationException("'Authentication:Jwt:Audience'");

        List<Claim> claims = 
            [
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Name, user.Username),
                new (ClaimTypes.Role, user.Admin ? UserAccess.Admin : UserAccess.User)
            ];
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}