namespace AnimeApi.Server.Core.Sorting;

/// <summary>
/// Holds the names of the fields that can be used for sorting.
/// </summary>
public static class SortConstants
{
    /// <summary>
    /// Anime sorting fields
    /// </summary>
    public static class Anime
    {
        public const string Id = "id";
        public const string Name = "title";
        public const string ReleaseYear = "year";
        public const string ReleaseDate = "release_date";
        public const string Score = "score";
        public const string Episodes = "episodes";
    }
    
    /// <summary>
    /// Review sorting fields
    /// </summary>
    public static class Review
    {
        public const string Id = "id";
        public const string Title = "title";
        public const string Score = "score";
        public const string Date = "date";
    }
    
    /// <summary>
    /// Constant representing the direction of sorting ascending.
    /// </summary>
    public const string Ascending = "asc";
    
    /// <summary>
    /// Constant representing the direction of sorting descending.
    /// </summary>
    public const string Descending = "desc";

    /// <summary>
    /// Constant representing the directions of sorting.
    /// </summary>
    public static readonly IReadOnlyList<string> Directions = 
    [
        Ascending,
        Descending
    ]; 
}