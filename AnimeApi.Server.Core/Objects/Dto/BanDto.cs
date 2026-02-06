namespace AnimeApi.Server.Core.Objects.Dto;

public record BanDto
{
    public int Id { get; init; }
    
    public int UserId { get; set; }
    
    public string NormalizedEmail { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime? Expiration { get; init; }
    
    public string? Reason { get; init; }
    
    public AppUserDto User { get; init; }
    
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