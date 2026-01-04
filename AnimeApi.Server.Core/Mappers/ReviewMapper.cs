using System.Linq.Expressions;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Objects.Partials;

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
          Title = review.Title,
          Content = review.Content,
          CreatedAt = review.CreatedAt,
          Score = review.Score,
          AnimeId = review.AnimeId,
          UserId = review.UserId,
        };
    }

    public override Review MapToEntity(ReviewDto review)
    {
        return new Review
        {
          Id = review.Id,
          Title = review.Title,
          Content = review.Content,
          CreatedAt = review.CreatedAt,
          Score = review.Score,
          AnimeId = review.AnimeId,
          UserId = review.UserId,
        };
    }

    public override Expression<Func<Review, TResult>> Projection<TResult>()
    {
        if (typeof(TResult) == typeof(ReviewDetailedDto))
        {
            Expression<Func<Review, ReviewDetailedDto>> expr =
                r => new ReviewDetailedDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt,
                    Score = r.Score,
                    AnimeId = r.AnimeId,
                    UserId = r.UserId,
                    User = new PublicUser
                    {
                        Id = r.User.Id,
                        Username = r.User.Username,
                        PictureUrl = r.User.PictureUrl
                    },
                    Anime = new AnimeSummary
                    {
                        Id = r.Anime.Id,
                        Name = r.Anime.Name,
                        ImageUrl = r.Anime.ImageUrl,
                        Score = r.Anime.Score,
                        Rating = r.Anime.Rating,
                        ReleaseYear = r.Anime.ReleaseYear
                    }
                };

            return expr as Expression<Func<Review, TResult>>;
        }
        
        return base.Projection<TResult>();
    }
}