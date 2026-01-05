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
    public ReviewMapper()
    {
        Profile(
            r => r.User,
            u => new PublicUser
            {
                Id = u.Id,
                Username = u.Username,
                PictureUrl =  u.PictureUrl,
            });
        
        Profile(
            r => r.Anime,
            a => new AnimeSummary
            {
                Id = a.Id,
                Name = a.Name,
                ImageUrl = a.ImageUrl,
                Score =  a.Score,
                Rating = a.Rating,
                ReleaseYear =  a.ReleaseYear,
            });
    }
    
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
}