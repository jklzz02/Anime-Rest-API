using AnimeApi.Server.Business.Objects.Dto.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators.Interfaces;

/// <summary>
/// Represents a contract to validate entities of a specific type
/// with optional support for using contextual data such as existing IDs or names during the validation process.
/// </summary>
/// <typeparam name="TEntity">The type of the entity that the validator is designed to validate.</typeparam>
public interface IBaseValidator<in TEntity> : IValidator<TEntity>
    where TEntity : IBaseDto
{
    /// <summary>
    /// Configures the validator to consider a provided collection of existing IDs during the validation process.
    /// </summary>
    /// <param name="ids">A collection of integers representing existing IDs to be accounted for in validation.</param>
    /// <returns>The instance of the validator with the updated configuration.</returns>
    IBaseValidator<TEntity> WithExistingIds(IEnumerable<int> ids);

    /// <summary>
    /// Configures the validator to consider a provided collection of existing names during the validation process.
    /// </summary>
    /// <param name="names">A collection of strings representing existing names to be accounted for in validation.</param>
    /// <returns>The instance of the validator with the updated configuration.</returns>
    IBaseValidator<TEntity> WithExistingNames(IEnumerable<string> names);
}