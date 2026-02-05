using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Specification;

public class FavouriteQuery : Specification<Favourite, FavouriteQuery>
{
    public FavouriteQuery ByUserId(int userId)
        => FilterBy(f => f.UserId == userId);
    
    public FavouriteQuery ByAnimeId(int animeId)
        => FilterBy(f => f.AnimeId == animeId);
}
