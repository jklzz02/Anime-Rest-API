using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Specification;

public class TokenQuery : Specification<RefreshToken, TokenQuery>
{
    public TokenQuery ByPk(int id)
        => FilterBy(t => t.Id == id);

    public TokenQuery ByToken(string token)
        => FilterBy(t => t.HashedToken == token);

    public TokenQuery ByUser(int userId)
        => FilterBy(t => t.UserId == userId);
}
