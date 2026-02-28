using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.RequestModels.Recommender;

public class CompatibilityAnimeRequest
{
    [FromQuery(Name = "target_anime_id"), Range(1, int.MaxValue)]
    public int AnimeId { get; init; }
    
    [FromQuery(Name = "user_favourite_ids")]
    public required IEnumerable<int> UserFavouriteIds { get; init; }
}