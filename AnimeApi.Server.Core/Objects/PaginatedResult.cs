using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects;

/// <summary>
/// Represents a paginated result object for handling collections of data with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of data contained in the paginated result. Must be a reference type.</typeparam>
public sealed class PaginatedResult<T> where T : class
{
    [JsonIgnore]
    public List<Error> Errors { get; } = [];
    
    [JsonIgnore]
    public List<Error> ValidationErrors => 
        Errors.Where(e => e.IsValidation).ToList();

    [JsonIgnore]
    public bool Success
        => !Errors.Any();

    [JsonProperty("page")]
    public int Page { get; }
   
    [JsonProperty("result_count")]
    public int ResultCount => Items.Count();
    
    [JsonProperty("page_size")]
    public int PageSize { get; }
    
    [JsonProperty("total_pages")]
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
   
    [JsonProperty("total_items")]
    public int TotalItems { get; }
   
    [JsonProperty("has_previous_page")]
    public bool HasPreviousPage => Page > 1;

    [JsonProperty("has_next_page")]
    public bool HasNextPage => Page < TotalPages;
    
    [JsonProperty("has_items")]
    public bool HasItems => ResultCount > 0;
    
    [JsonProperty("data")]
    public IEnumerable<T> Items { get; }
    
    public PaginatedResult(Error error)
    {
        Errors.Add(error);
        Items = [];
        Page = 1;
        TotalItems = 0;
        PageSize = 0;
    }

    public PaginatedResult(List<Error> errors)
    {
        Errors = errors;
        Items = [];
        Page = 1;
        TotalItems = 0;
        PageSize = 0;
    }

    public PaginatedResult(IEnumerable<T> items, int page, int size)
    {
        Items = items;
        Page = page;
        TotalItems = 0;
        PageSize = size;
    }

    public PaginatedResult(IEnumerable<T> items, int page, int size, int totalItems)
    {
        Items = items;
        Page = page;
        TotalItems = totalItems;
        PageSize = size;
    }
}