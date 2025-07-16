
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Extensions;

/// <summary>
/// Provides extension methods for managing collections of <see cref="IAnimeRelation"/> objects.
/// This class includes utilities for comparing and updating anime relation collections.
/// </summary>
public static class AnimeRelationExtension
{
    /// <summary>
    /// Determines whether two collections of <see cref="IAnimeRelation"/> are equal.
    /// Collections are considered equal if they have the same number of elements,
    /// and all elements' RelatedId properties match in order when sorted.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the collections, implementing <see cref="IAnimeRelation"/>.</typeparam>
    /// <param name="relations">The primary collection of <see cref="IAnimeRelation"/> objects to compare.</param>
    /// <param name="otherRelations">The collection of <see cref="IAnimeRelation"/> objects to compare against.</param>
    /// <returns>True if the collections are equal, otherwise false.</returns>
    public static bool IsEqualTo<T>(
        this ICollection<T> relations,
        ICollection<T> otherRelations
        )
        where T : class, IAnimeRelation, new()
    {
        if (relations.Count != otherRelations.Count) return false;
        
        var originalRelationsIds = relations
            .Select(og => og.RelatedId)
            .OrderBy(a => a)
            .ToList();
        
        var otherRelationsIds = otherRelations
            .Select(ot => ot.RelatedId)
            .OrderBy(a => a)
            .ToList();
        
        return originalRelationsIds.SequenceEqual(otherRelationsIds);
    }

    /// <summary>
    /// Updates the current collection of <see cref="IAnimeRelation"/> objects to match the given updated collection.
    /// If the collections are not equal, the original collection is cleared and populated with new elements
    /// containing the specified anime identifier and related IDs from the updated collection.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the collection, implementing <see cref="IAnimeRelation"/>.</typeparam>
    /// <param name="relations">The original collection of <see cref="IAnimeRelation"/> objects to be updated.</param>
    /// <param name="updatedRelations">The updated collection of <see cref="IAnimeRelation"/> objects to sync with.</param>
    /// <param name="animeId">The identifier of the anime to assign to each element while updating the collection.</param>
    public static void Update<T>(
        this ICollection<T> relations,
        ICollection<T> updatedRelations,
        int animeId
        )
        where T : class, IAnimeRelation, new()
    {
        if (!relations.IsEqualTo(updatedRelations))
        {
            relations.Clear();
            foreach (var updatedRelation in updatedRelations)
            {
                relations.Add(new T{AnimeId = animeId, RelatedId = updatedRelation.RelatedId});
            }
        }
    }
}