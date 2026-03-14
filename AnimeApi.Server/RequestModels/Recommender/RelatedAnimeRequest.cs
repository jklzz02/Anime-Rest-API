using System.ComponentModel.DataAnnotations;
using AnimeApi.Server.RequestModels.ResultType;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.RequestModels.Recommender;

public class RelatedAnimeRequest
{
    [FromQuery(Name = "anime_id"), Range(1, int.MaxValue)]
    public int AnimeId { get; init; }
    
    [FromQuery(Name = "count"), Range(1, 100)]
    public int Count { get; init; } = 10;
    
    [FromQuery(Name = "type")]
    public AnimeResultType EntityType { get; init; } = AnimeResultType.Full;
}