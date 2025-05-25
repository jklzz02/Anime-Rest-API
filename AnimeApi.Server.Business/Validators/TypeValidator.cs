using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Validators.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class TypeValidator : AbstractValidator<TypeDto>, ITypeValidator
{
    public TypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name cannot be empty")
            .MaximumLength(50)
            .WithMessage("The name cannot be longer than 50 characters");
    }

    public ITypeValidator WithExistingIds(IEnumerable<int> ids)
    {
        RuleFor(x => x.Id)
            .Must(id => !ids.Contains(id!.Value))
            .WithMessage(x => $"There's already a type with id '{x.Id}'");
        
        return this;
    }

    public ITypeValidator WithExistingNames(IEnumerable<string> names)
    {
        RuleFor(x => x.Name)
            .Must(name => !names.Contains(name))
            .WithMessage(x => $"There's already a type with name '{x.Name}'");
        
        return this;
    }
}