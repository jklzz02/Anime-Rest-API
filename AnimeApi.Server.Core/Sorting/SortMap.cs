using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using System.Diagnostics.CodeAnalysis;

namespace AnimeApi.Server.Core.Sorting;

public class SortMap<TEntity>
    where TEntity : class, new()
{
    private readonly Dictionary<string, SortAction<TEntity>> _map = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<string> AvailableFields
        => _map.Keys.ToList();

    public bool IsValid(string? fieldName)
        => fieldName != null && _map.ContainsKey(fieldName);

    public static SortAction<TEntity>? Action(string fieldName)
        => new SortMap<TEntity>().TryGetAction(fieldName, out var sortAction)
            ? sortAction
            : null;

    public static SortAction<TEntity>? Action(string fieldName, bool asc)
    {
        var sortAction = Action(fieldName);

        if (sortAction == null)
        {
            return null;
        }
        
        return asc
            ? SortAction<TEntity>.Asc(sortAction.KeySelector)
            : SortAction<TEntity>.Desc(sortAction.KeySelector);
    }

    public bool TryGetAction(string fieldName, [NotNullWhen(true)] out SortAction<TEntity>? sortAction)
        => _map.TryGetValue(fieldName, out sortAction);
    
    public static IReadOnlyList<string> Fields
        => new SortMap<TEntity>().AvailableFields;
    
    public static bool Validate(string? fieldName)
        => new SortMap<TEntity>().IsValid(fieldName);

    protected void Register(string fieldName, SortAction<TEntity> sortAction)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);
        ArgumentNullException.ThrowIfNull(sortAction);

        if (!_map.TryAdd(fieldName, sortAction))
        {
            throw new ArgumentException($"Sorting field '{fieldName}' is already registered");
        }
    }
}