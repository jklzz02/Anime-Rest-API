using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.SpecHelpers;

public class TokenQuery : QuerySpec<RefreshToken, TokenQuery>
{
    public TokenQuery ByPk(int id)
        => FilterBy(t => t.Id == id);

    public TokenQuery ByToken(string token)
        => FilterBy(t => t.Hashed_Token == token);

    public TokenQuery ByUser(int userId)
        => FilterBy(t => t.User_Id == userId);
}
