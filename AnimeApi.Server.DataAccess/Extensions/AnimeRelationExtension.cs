using AnimeApi.Server.DataAccess.Models.Interfaces;

namespace AnimeApi.Server.DataAccess.Extensions;

public static class AnimeRelationExtension
{
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