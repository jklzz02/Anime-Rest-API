using AnimeApi.Server.Business.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators.Interfaces;

public interface IReviewValidator : IValidator<ReviewDto>
{
}