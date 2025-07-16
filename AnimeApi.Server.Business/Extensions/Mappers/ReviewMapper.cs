using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

/// <summary>
/// Provides mapping extensions for converting between <see cref="Review"/> and <see cref="ReviewDto"/> entities.
/// </summary>
public static class ReviewMapper
{
    public static ReviewDto ToDto(this Review review)
    {
        return new ReviewDto
        {
          Id = review.Id,
          Content = review.Content ?? string.Empty,
          CreatedAt = review.Created_At,
          Score = review.Score,
          AnimeId = review.Anime_Id,
          UserId = review.User_Id,
        };
    }

    public static Review ToModel(this ReviewDto review)
    {
        return new Review
        {
          Id = review.Id,
          Content = review.Content,
          Created_At = review.CreatedAt,
          Score = review.Score,
          Anime_Id = review.AnimeId,
          User_Id = review.UserId,
        };
    }

    public static IEnumerable<ReviewDto> ToDto(this IEnumerable<Review> reviews)
    {
        return reviews.Select(r => r.ToDto());
    }

    public static IEnumerable<Review> ToModel(this IEnumerable<ReviewDto> reviews)
    {
        return reviews.Select(r => r.ToModel());
    }
}