using AnimeApi.Server.Business.Dto.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class BaseValidator<TEntity>  : AbstractValidator<TEntity>, IBaseValidator<TEntity>
    where TEntity : IBaseDto
{ 
    protected string EntityName => typeof(TEntity).Name
        .ToLower()
        .Replace("dto", "");

    public BaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name cannot be empty")
            .MaximumLength(50)
            .WithMessage("The name cannot be longer than 50 characters");
    }
    public IBaseValidator<TEntity> WithExistingIds(IEnumerable<int> ids)
    {
        RuleFor(x => x.Id)
            .Must(id => !ids.Contains(id ?? 0))
            .WithMessage(x => $"There's already another {EntityName} with id '{x.Id}'");
        
        return this;
    }

    public IBaseValidator<TEntity> WithExistingNames(IEnumerable<string> names)
    {
        RuleFor(x => x.Name)
            .Must(name => !names.Contains(name))
            .WithMessage(x => $"There's already another {EntityName} with name '{x.Name}'");
        
        return this;
    }
}