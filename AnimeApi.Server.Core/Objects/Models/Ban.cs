namespace AnimeApi.Server.Core.Objects.Models;

public class Ban
{
    public int Id { get; set; }
    
    public string NormalizedEmail { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? Expiration { get; set; }
    
    public string? Reason { get; set; }
    
    public AppUser User { get; set; }
}