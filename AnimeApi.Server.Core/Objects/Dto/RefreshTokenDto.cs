using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record RefreshTokenDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("hashed_token")]
    public string HashedToken { get; set; } = null!;
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("expires_at")]
    public DateTime ExpiresAt { get; set; }
    [JsonProperty("revoked_at")]
    public DateTime? RevokedAt { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
   
    [JsonIgnore]
    public bool IsRevoked 
        => RevokedAt.HasValue;
   
    [JsonIgnore]
    public bool IsExpired 
        => DateTime.UtcNow >= ExpiresAt;
   
    [JsonIgnore]
    public bool IsActive 
        => !IsRevoked && !IsExpired;

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow.ToUniversalTime();
    }
}