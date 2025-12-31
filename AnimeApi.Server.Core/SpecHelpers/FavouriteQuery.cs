using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.SpecHelpers;

public class FavouriteQuery : QuerySpec<Favourite, FavouriteQuery>
{
    public FavouriteQuery ByUserId(int userId)
        => FilterBy(f => f.User_Id == userId);
    
    public FavouriteQuery ByAnimeId(int animeId)
        => FilterBy(f => f.Anime_Id == animeId);
}
