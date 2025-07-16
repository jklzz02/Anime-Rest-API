namespace AnimeApi.Server.DataAccess.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public DateTime Created_At { get; set; }
    public string Picture_Url { get; set; } = null!;
    public int Role_Id { get; set; }
    public Role Role { get; set; } = null!;
    
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}