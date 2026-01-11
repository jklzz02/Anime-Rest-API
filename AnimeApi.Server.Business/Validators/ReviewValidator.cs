using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class ReviewValidator : AbstractValidator<ReviewDto>
{
    public ReviewValidator()
    {
        RuleFor(r => r.Id)
            .GreaterThan(0)
            .When(r => r.Id != 0)
            .WithMessage("must be greater than 0");
        
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

        RuleFor(r => r.Title)
            .NotEmpty()
            .WithMessage("cannot be empty")
            .MinimumLength(10)
            .WithMessage("title must be at least 10 characters long")
            .MaximumLength(30)
            .WithMessage("title cannot be longer than 30 characters");
        
        RuleFor(r => r.Content)
            .NotEmpty()
            .MinimumLength(100)
            .WithMessage("content must be at least 100 characters long")
            .WithMessage("cannot be empty")
            .MaximumLength(5000)
            .WithMessage("cannot be longer than 5000 characters");
    }
}