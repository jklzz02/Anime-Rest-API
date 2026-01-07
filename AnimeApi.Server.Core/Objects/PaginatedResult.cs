using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects;

/// <summary>
/// Represents a paginated result object for handling collections of data with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of data contained in the paginated result. Must be a reference type.</typeparam>
public sealed class PaginatedResult<T> where T : class
{
    private readonly List<Error> _errors = [];
    
    [JsonIgnore]
    public IReadOnlyList<Error> Errors
        => _errors;
    
    [JsonIgnore]
    public List<Error> ValidationErrors
        => Errors
            .Where(e => e.IsValidation)
            .ToList();

    [JsonIgnore]
    public bool Success
        => !Errors.Any();

    [JsonProperty("page")]
    public int Page { get; }
   
    [JsonProperty("result_count")]
    public int ResultCount
        => Items.Count();
    
    [JsonProperty("page_size")]
    public int PageSize { get; }
    
    [JsonProperty("total_pages")]
    public int TotalPages
        =>  PageSize > 0 &&  TotalItems > 0
            ? (int) Math.Ceiling(TotalItems / (double) PageSize)
            : 0;
   
    [JsonProperty("total_items")]
    public int TotalItems { get; }
   
    [JsonProperty("has_previous_page")]
    public bool HasPreviousPage
        => Page > 1;

    [JsonProperty("has_next_page")]
    public bool HasNextPage
        => Page < TotalPages;
    
    [JsonProperty("has_items")]
    public bool HasItems
        => ResultCount > 0;
    
    [JsonProperty("data")]
    public IEnumerable<T> Items { get; }
    
    public PaginatedResult(Error error)
        : this([error])
    {
    }

    public PaginatedResult(List<Error> errors)
    {
        _errors = errors;
        Items = [];
        Page = 0;
        TotalItems = 0;
        PageSize = 0;
    }

    public PaginatedResult(IEnumerable<T> items, int page, int size)
        : this(items, page, size, 0)
    {
    }

    public PaginatedResult(IEnumerable<T> items, int page, int size, int totalItems)
    {
        if (page <= 0)
        {
            _errors.Add(Error.Validation("page", "must be greater than 0."));
        }
        
        switch (size)
        {
            case <= 0:
                _errors.Add(Error.Validation("size", "must be greater than 0."));
                break;
            
            case < Constants.Pagination.MinPageSize:
                _errors.Add(Error.Validation(
                    "size", 
                    $"must be greater than or equal to {Constants.Pagination.MinPageSize}."));
                break;
            
            case > Constants.Pagination.MaxPageSize:
                _errors.Add(Error.Validation(
                    "size", 
                    $"must be less than or equal to {Constants.Pagination.MaxPageSize}."));
                break;
        }
        
        Items = items;
        Page = page;
        TotalItems = totalItems;
        PageSize = size;
    }
}