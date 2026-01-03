using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Exceptions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;
using AnimeApi.Server.Core.Objects.Dto;
using Google.Apis.Auth;
using Newtonsoft.Json;

namespace AnimeApi.Server;

public class IdentityProviderService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    
    public IdentityProviderService(
        IConfiguration configuration,
        IHttpClientFactory clientFactory, 
        IUserService userService)
    {
        ConfigurationException.ThrowIfMissing(configuration, "Authentication:Facebook");
        ConfigurationException.ThrowIfEmpty(configuration, "Authentication:Facebook:AppId");
        
        _configuration = configuration;
        _clientFactory = clientFactory;
        _userService = userService;
    }
    
    public async Task<Result<AppUserDto>> ProcessIdentityProviderAsync(AuthRequest request)
    {
        var result = request.Provider switch
        {
            Constants.Auth.IdentityProvider.Google
                => await ProcessGoogleAuthAsync(request),
            
            Constants.Auth.IdentityProvider.Facebook  
                => await ProcessFacebookAuthAsync(request),
            
            _ => Result<AppUserDto>.ValidationFailure("Unauthorized", "Invalid identity provider.")
        };
        
        return result;
    }
    
    private async Task<Result<AppUserDto>> ProcessFacebookAuthAsync(AuthRequest request)
    {
        List<Error> errors = [];

        if (string.IsNullOrEmpty(request.Code))
        {
            errors.Add(Error.Validation("Unauthorized", "Missing Facebook Authorization code."));
        }
        
        if (string.IsNullOrEmpty(request.RedirectUri))
        {
            errors.Add(Error.Validation("Unauthorized", "Missing Facebook Redirect URI."));
        }

        if (string.IsNullOrEmpty(request.CodeVerifier))
        {
            errors.Add(Error.Validation("Unauthorized", "Missing Facebook Code Verifier."));
        }

        if (errors.Any())
        {
            return Result<AppUserDto>.Failure(errors);
        }
        
        var appId = _configuration["Authentication:Facebook:AppId"];
        
        var client = _clientFactory.CreateClient();
        var tokenResponse = await client.PostAsync(
            "https://graph.facebook.com/v17.0/oauth/access_token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = appId!,
                ["redirect_uri"] = request.RedirectUri!,
                ["code_verifier"] = request.CodeVerifier!,
                ["code"] = request.Code!,
                ["grant_type"] = "authorization_code"
            })
        );

        var content = await tokenResponse.Content.ReadAsStringAsync();
        var tokenData = JsonConvert.DeserializeObject<FacebookToken>(content);

        if (tokenData is null)
        {
            return Result<AppUserDto>.ValidationFailure("Unauthorized", "Invalid Facebook Access token.");
        }

        var userResponse = await client.GetAsync(
            $"https://graph.facebook.com/v24.0/me?fields=name,email,picture&access_token={tokenData.AccessToken}"
        );

        var userContent = await userResponse.Content.ReadAsStringAsync();
        var fbUser = JsonConvert.DeserializeObject<FacebookResponse>(userContent);

        if (fbUser is null)
        {
            return Result<AppUserDto>.ValidationFailure("Unauthorized", "Invalid Facebook Access token.");
        }
        
        var user = await _userService.GetOrCreateUserAsync(new AuthPayload
        {
            Email = fbUser.Email,
            Picture = fbUser.Picture.Data.Url,
            Username = fbUser.Name
        });

        return Result<AppUserDto>.Success(user);
    }
    
    private async Task<Result<AppUserDto>> ProcessGoogleAuthAsync(AuthRequest request)
    {
        if (string.IsNullOrEmpty(request.Token))
        {
            return Result<AppUserDto>.ValidationFailure("Unauthorized", "Missing Google ID token.");
        }
        
        try
        {
            GoogleJsonWebSignature.Payload payload = await 
                GoogleJsonWebSignature.ValidateAsync(request.Token);
            
            var userDto = await 
                _userService.GetOrCreateUserAsync(new AuthPayload
                {
                    Email = payload.Email,
                    Picture = payload.Picture,
                    Username = string.Empty
                });
            
            return Result<AppUserDto>.Success(userDto);
        }
        catch (InvalidJwtException)
        {
            return Result<AppUserDto>.ValidationFailure("Unauthorized", "Invalid Google ID token.");
        }
    }
}