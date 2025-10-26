using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Mappers;

/// <summary>
/// Provides mapping extensions for converting between <see cref="Review"/> and <see cref="ReviewDto"/> entities.
/// </summary>
public class ReviewMapper : Mapper<Review, ReviewDto>
{
    public override ReviewDto MapToDto(Review review)
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

    public override Review MapToEntity(ReviewDto review)
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
}