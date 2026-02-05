using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Specification;

public class BanQuery : Specification<Ban, BanQuery>
{
    public BanQuery Active()
        => FilterBy(b => b.Expiration == null || b.Expiration > DateTime.UtcNow);
    
    public BanQuery Expired()
        => FilterBy(b => b.Expiration < DateTime.UtcNow);
    
    public BanQuery ByUser(int userId)
    => FilterBy(b => b.UserId == userId);
    
    public BanQuery ByUser(string email)
        => FilterBy(b => b.NormalizedEmail == email);
}