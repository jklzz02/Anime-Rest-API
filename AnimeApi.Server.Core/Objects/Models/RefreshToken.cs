namespace AnimeApi.Server.Core.Objects.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string HashedToken { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
}