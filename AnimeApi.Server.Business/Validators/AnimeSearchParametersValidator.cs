using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Sorting;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class AnimeSearchParametersValidator : AbstractValidator<AnimeSearchParameters>
{
    public AnimeSearchParametersValidator()
    {
        RuleFor(a => a.OrderBy)
            .Must(AnimeSortMap.Validate)
            .When(a => !string.IsNullOrEmpty(a.OrderBy))
            .WithMessage($"Invalid order by field. Choose among: ({string.Join(", ", AnimeSortMap.Fields)})");
        
        RuleFor(a => a.SortOrder)
            .Must(a => SortConstants.Directions.Contains(a))
            .When(a => !string.IsNullOrEmpty(a.SortOrder))
            .WithMessage($"Invalid sort order. Choose among: ({string.Join(", ", SortConstants.Directions)})");
    }
}