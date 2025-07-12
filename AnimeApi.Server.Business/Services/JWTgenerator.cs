using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.Business.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static AnimeApi.Server.Business.Constants;

namespace AnimeApi.Server.Business.Services;

public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _configuration;

    public JwtGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(AppUserDto user)
    {
        var secrets = _configuration.GetSection("Authentication:Jwt");

        var secret = secrets.GetSection("Secret").Value ?? 
                     throw new ApplicationException("Secret not found in configuration");
        
        var issuer = secrets.GetSection("Issuer").Value ??
                     throw new ApplicationException("Issuer not found in configuration");
        
        var audience = secrets.GetSection("Audience").Value ??
                       throw new ApplicationException("Audience not found in configuration");

        List<Claim> claims = 
            [
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Name, user.Username),
                new (UserAccess.User, user.Admin.ToString())
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