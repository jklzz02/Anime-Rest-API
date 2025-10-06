using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Objects;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class AnimeSearchParametersValidator : AbstractValidator<AnimeSearchParameters>
{
    private readonly IEnumerable<string> _validOrderByEntries =
        typeof(Constants.OrderBy.Fields)
            .GetFields()
            .Select(f => f.GetRawConstantValue()?.ToString()!);

    private readonly IEnumerable<string> _validSortOrderEntries =
        typeof(Constants.OrderBy.Directions)
            .GetFields()
            .Select(f => f.GetRawConstantValue()?.ToString()!);

    public AnimeSearchParametersValidator()
    {
        RuleFor(a => a.OrderBy)
            .Must(a => _validOrderByEntries.Contains(a))
            .When(a => !string.IsNullOrEmpty(a.OrderBy))
            .WithMessage($"Invalid order by field. Choose among: ({string.Join(", ", _validOrderByEntries)})");
        
        RuleFor(a => a.SortOrder)
            .Must(a => _validSortOrderEntries.Contains(a))
            .When(a => !string.IsNullOrEmpty(a.SortOrder))
            .WithMessage($"Invalid sort order. Choose among: ({string.Join(", ", _validSortOrderEntries)})");
    }
}