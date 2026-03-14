using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Sorting;

public class ReviewSortMap : SortMap<Review>
{
    public ReviewSortMap()
    {
        Register(SortConstants.Review.Id, SortAction<Review>.Asc(r => r.Id));
        Register(SortConstants.Review.Title, SortAction<Review>.Asc(r => r.Title));
        Register(SortConstants.Review.Score, SortAction<Review>.Desc(r => r.Score));
        Register(SortConstants.Review.Date, SortAction<Review>.Desc(r => r.CreatedAt));
    }
}