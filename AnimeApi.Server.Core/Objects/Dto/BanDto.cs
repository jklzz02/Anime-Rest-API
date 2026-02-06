using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record BanDto
{
    [JsonProperty("id")]
    public int Id { get; init; }
    
    [JsonProperty("user_id")]
    public int UserId { get; set; }
    
    [JsonProperty("normalized_email")]
    public string NormalizedEmail { get; init; }
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; init; }
    
    [JsonProperty("expiration_date")]
    public DateTime? Expiration { get; init; }
    
    [JsonProperty("reason")]
    public string? Reason { get; init; }
    
    [JsonIgnore]
    public AppUserDto? User { get; init; }
    
    [JsonProperty("active")]
    public bool IsActive
        => Expiration is null || Expiration > DateTime.UtcNow;

    public BanDto Updated(string? reason, DateTime? expiration)
        => new BanDto
        {
            Id = Id,
            UserId = UserId,
            NormalizedEmail = NormalizedEmail,
            CreatedAt = DateTime.UtcNow,
            Expiration = expiration,
            Reason = reason
        };
    
    public BanDto Revoked()
        => new BanDto
        {
            Id = Id,
            UserId = UserId,
            CreatedAt = CreatedAt,
            NormalizedEmail =  NormalizedEmail,
            Expiration = DateTime.UtcNow,
            Reason = Reason
        };
}