using AnimeApi.Server.Core.Extensions;

namespace AnimeApi.Server.Core.Objects.Auth;

public record AuthPayload
{
    private readonly string _username = string.Empty;

    public string Username
    {
        get => string.IsNullOrWhiteSpace(_username)
            ? Email.EmailToUsername()
            : _username;
        
        init => _username = value;
    }
    
    public string Email { get; init; } = string.Empty;
    public string Picture { get; init; } = string.Empty;
}
