using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Sorting;

public class ReviewSortMap : SortMap<Review, ReviewSortMap>
{
    public ReviewSortMap()
    {
        Register(SortConstants.Review.Id, r => r.Id);
        Register(SortConstants.Review.Title, r => r.Title);
        Register(SortConstants.Review.Score, r => r.Score);
        Register(SortConstants.Review.Date, r => r.CreatedAt);
    }
}