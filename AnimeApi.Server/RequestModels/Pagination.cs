using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using static AnimeApi.Server.Core.Constants.Pagination;

namespace AnimeApi.Server.RequestModels;

public record Pagination
{
    [FromQuery(Name = "page"), Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [FromQuery(Name = "size"), Range(MinPageSize, MaxPageSize)]
    public int Size { get; init; } = MinPageSize;
}