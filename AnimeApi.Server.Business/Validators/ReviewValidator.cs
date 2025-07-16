using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class ReviewValidator : AbstractValidator<ReviewDto>, IReviewValidator
{
    public ReviewValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage("cannot be empty");
        
        RuleFor(r => r.AnimeId)
            .NotEmpty()
            .WithMessage("cannot be empty")
            .GreaterThan(0)
            .WithMessage("must be greater than 0");
        
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("cannot be empty")
            .GreaterThan(0)
            .WithMessage("must be greater than 0");
        
        RuleFor(r => r.Score)
            .NotEmpty()
            .WithMessage("cannot be empty")
            .GreaterThanOrEqualTo(0)
            .WithMessage("must be greater than or equal to 0")
            .LessThanOrEqualTo(10)
            .WithMessage("must be less than or equal to 10");
        
        RuleFor(r => r.Content)
            .NotEmpty()
            .WithMessage("cannot be empty")
            .MaximumLength(5000)
            .WithMessage("cannot be longer than 5000 characters");
    }
}