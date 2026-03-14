using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Sorting;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class ReviewSearchParametersValidator : AbstractValidator<ReviewSearchParameters>
{
    public ReviewSearchParametersValidator()
    {
        RuleFor(r => r.AnimeId)
            .GreaterThan(0)
            .When(r => r.AnimeId != null)
            .WithMessage("must be greater than 0");
        
        RuleFor(r => r.UserId)
            .GreaterThan(0)
            .When(r => r.UserId != null)
            .WithMessage("must be greater than 0");
        
        RuleFor(r => r.OrderBy)
            .Must(ReviewSortMap.Validate)
            .When(r => !string.IsNullOrEmpty(r.OrderBy))
            .WithMessage($"Invalid order by field. Choose among: ({string.Join(", ", ReviewSortMap.Fields)})");
        
        RuleFor(r => r.From)
            .Must(r => r < DateTime.UtcNow)
            .When(r => r.From.HasValue)
            .WithMessage("must be in the past");
        
        RuleFor(r => r.SortOrder)
            .Must(a => SortConstants.Directions.Contains(a))
            .When(r => !string.IsNullOrEmpty(r.SortOrder))
            .WithMessage($"Invalid sort order. Choose among: ({string.Join(", ", SortConstants.Directions)})");
    }
}