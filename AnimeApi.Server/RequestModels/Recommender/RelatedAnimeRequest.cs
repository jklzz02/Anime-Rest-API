using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.RequestModels.Recommender;

public class RelatedAnimeRequest
{
    [FromQuery(Name = "anime_id"), Range(1, int.MaxValue)]
    public int AnimeId { get; init; }
    
    [FromQuery(Name = "count"), Range(1, 100)]
    public int Count { get; init; } = 10;
    
    [FromQuery(Name = "type")]
    public ResultEntityType EntityType { get; init; } = ResultEntityType.Full;
}