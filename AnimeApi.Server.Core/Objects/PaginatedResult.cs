using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects;

public class PaginatedResult<T> where T : class
{
    [JsonProperty("page")]
    public int Page { get; }
   
    [JsonProperty("page_items")]
    public int PageSize => Items.Count();
    
    [JsonProperty("total_pages")]
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
   
    [JsonProperty("total_items")]
    public int TotalItems { get; }
   
    [JsonProperty("has_previous_page")]
    public bool HasPreviousPage => Page > 1;

    [JsonProperty("has_next_page")]
    public bool HasNextPage => Page < TotalPages;
    
    [JsonProperty("has_items")]
    public bool HasItems => PageSize > 0;
    
    [JsonProperty("data")]
    public IEnumerable<T> Items { get; }
    
    public PaginatedResult(IEnumerable<T> items, int page)
    {
        Items = items;
        Page = page;
        TotalItems = 0;
    }

    public PaginatedResult(IEnumerable<T> items, int page, int totalItems)
    {
        Items = items;
        Page = page;
        TotalItems = totalItems;
    }
}