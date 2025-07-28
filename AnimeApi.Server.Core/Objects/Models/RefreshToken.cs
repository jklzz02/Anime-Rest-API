namespace AnimeApi.Server.Core.Objects.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Hashed_Token { get; set; } = null!;
    public DateTime Created_At { get; set; }
    public DateTime Expires_At { get; set; }
    public DateTime? Revoked_At { get; set; }
    public int User_Id { get; set; }
    public AppUser User { get; set; } = null!;
}