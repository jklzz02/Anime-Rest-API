using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Objects;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class AnimeSearchParametersValidator : AbstractValidator<AnimeSearchParameters>
{
    public AnimeSearchParametersValidator()
    {
        RuleFor(a => a.OrderBy)
            .Must(a => Constants.OrderBy.Fields.ValidFields.Contains(a))
            .When(a => !string.IsNullOrEmpty(a.OrderBy))
            .WithMessage($"Invalid order by field. Choose among: ({string.Join(", ", Constants.OrderBy.Fields.ValidFields)})");
        
        RuleFor(a => a.SortOrder)
            .Must(a => Constants.OrderBy.StringDirections.Directions.Contains(a))
            .When(a => !string.IsNullOrEmpty(a.SortOrder))
            .WithMessage($"Invalid sort order. Choose among: ({string.Join(", ", Constants.OrderBy.StringDirections.Directions)})");
    }
}