using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects;

public class ReviewSearchParameters
{
    [FromQuery(Name = "title")]
    public string? Title { get; set; }
    
    [FromQuery(Name = "anime_id")]
    public int? AnimeId { get; set; }
    
    [FromQuery(Name = "anime_title")]
    public string? AnimeName { get; set; }
    
    [FromQuery(Name = "user_id")]
    public int? UserId { get; set; }
    
    [FromQuery(Name = "user_name")]
    public string? UserName { get; set; }
    
    [FromQuery(Name = "from")]
    public DateTime? From { get; set; }
    
    [FromQuery(Name = "to")]
    public DateTime? To { get; set; }
    
    [FromQuery(Name = "min_score")]
    public decimal? MinScore { get; set; }
    
    [FromQuery(Name = "max_score")]
    public decimal? MaxScore { get; set; }
    
    [JsonProperty("order_by")]
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; }
    
    [JsonProperty("sort_order")]
    [FromQuery(Name = "sort_order")]
    public string? SortOrder { get; set; }
}