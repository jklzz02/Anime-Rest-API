using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.RequestModels;

public record AnimeListQuery
{
    [FromQuery(Name = "q"), MaxLength(30)]
    public string? Query { get; init; }
    
    [FromQuery(Name = "count"), Range(1, int.MaxValue)]
    public int Count { get; init; } = 200;
}