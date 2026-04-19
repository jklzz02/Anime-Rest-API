using AnimeApi.Server.Core.Extensions;

namespace AnimeApi.Server.Core.Objects.Auth;

public record AuthPayload
{
    public string Username
    {
        get;
        
        init => field = string.IsNullOrWhiteSpace(value)
            ? Email.EmailToUsername()
            : value;
        
    } = string.Empty;

    public Constants.Auth.IdentityProvider IdentityProvider { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Picture { get; init; } = string.Empty;
}
