using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Validators.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class SourceValidator : AbstractValidator<SourceDto>, ISourceValidator
{
    public SourceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name cannot be empty")
            .MaximumLength(50)
            .WithMessage("The name cannot be longer than 50 characters");
    }

    public ISourceValidator WithExistingIds(IEnumerable<int> ids)
    {
        RuleFor(x => x.Id)
            .Must(id => !ids.Contains(id ?? 0))
            .WithMessage(x => $"There's already an anime source with id '{x.Id}'");
        
        return this;
    }

    public ISourceValidator WithExistingNames(IEnumerable<string> names)
    {
        RuleFor(x => x.Name)
            .Must(name => !names.Contains(name))
            .WithMessage(x => $"There's already an anime source with name '{x.Name}'");
        
        return this;
    }
}