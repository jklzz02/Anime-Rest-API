using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.Core.Abstractions.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

/// <summary>
/// Provides a base implementation of a validator for DTO entities, enforcing common validation rules
/// such as name requirements and uniqueness constraints.
/// </summary>
/// <typeparam name="TEntity">The type of entity to be validated, which must implement IBaseDto.</typeparam>
public class BaseValidator<TEntity>  : AbstractValidator<TEntity>, IBaseValidator<TEntity>
    where TEntity : IBaseDto
{ 
    /// <summary>
    /// Gets the entity name by converting the type name to lowercase and removing the "dto" suffix.
    /// Used in validation messages to provide context-specific error descriptions.
    /// </summary>
    protected string EntityName => typeof(TEntity).Name
        .ToLower()
        .Replace("dto", "");

    /// <summary>
    /// Initializes a new instance of the BaseValidator class and sets up common validation rules.
    /// </summary>
    public BaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name cannot be empty")
            .MaximumLength(50)
            .WithMessage("The name cannot be longer than 50 characters");
    }
    /// <summary>
    /// Adds validation rule to ensure the entity's ID is unique within the provided collection of existing IDs.
    /// </summary>
    /// <param name="ids">Collection of existing entity IDs to check against.</param>
    /// <returns>The validator instance for method chaining.</returns>
    public IBaseValidator<TEntity> WithExistingIds(IEnumerable<int> ids)
    {
        RuleFor(x => x.Id)
            .Must(id => !ids.Contains(id ?? 0))
            .WithMessage(x => $"There's already another {EntityName} with id '{x.Id}'");
        
        return this;
    }

    /// <summary>
    /// Adds validation rule to ensure the entity's Name is unique within the provided collection of existing names.
    /// </summary>
    /// <param name="names">Collection of existing entity names to check against.</param>
    /// <returns>The validator instance for method chaining.</returns>
    public IBaseValidator<TEntity> WithExistingNames(IEnumerable<string> names)
    {
        RuleFor(x => x.Name)
            .Must(name => !names.Contains(name))
            .WithMessage(x => $"There's already another {EntityName} with name '{x.Name}'");
        
        return this;
    }
}