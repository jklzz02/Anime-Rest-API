using AnimeApi.Server.Business.Dto.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators.Interfaces;

/// <summary>
/// Defines a generic interface for entity validation with the ability to add additional contextual data
/// for validation processes. The implementation is expected to validate the specified entity type
/// and allow customization based on existing identifiers or names.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to be validated.</typeparam>
/// <typeparam name="TSelf">The type of the implementing class.</typeparam>
public interface IBaseValidator<in TEntity> : IValidator<TEntity>
    where TEntity : IBaseDto
{
    /// <summary>
    /// Updates the validator with a collection of existing IDs to ensure that the validation process
    /// accounts for these IDs when performing checks.
    /// </summary>
    /// <param name="ids">A collection of integers representing existing IDs that should be considered during validation.</param>
    /// <returns>Returns the instance of the implementing validator with the updated configuration.</returns>
    IBaseValidator<TEntity> WithExistingIds(IEnumerable<int> ids);

    /// <summary>
    /// Updates the validator with a collection of existing names to ensure that the validation process
    /// accounts for these names when performing checks.
    /// </summary>
    /// <param name="names">A collection of strings representing existing names that should be considered during validation.</param>
    /// <returns>Returns the instance of the implementing validator with the updated configuration.</returns>
    IBaseValidator<TEntity> WithExistingNames(IEnumerable<string> names);
}