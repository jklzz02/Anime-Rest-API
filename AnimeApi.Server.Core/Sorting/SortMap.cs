using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace AnimeApi.Server.Core.Sorting;

public class SortMap<TEntity, TDerived>
    where TEntity : class, new()
    where TDerived : SortMap<TEntity, TDerived>, new()
{
    private static readonly TDerived _instance = new(); 
    
    private readonly Dictionary<string, Expression<Func<TEntity, object?>>> _map =
        new(StringComparer.OrdinalIgnoreCase);

    private IReadOnlyList<string> AvailableFields
        => _map.Keys.ToList();

    private bool IsValid(string? fieldName)
        => fieldName != null && _map.ContainsKey(fieldName);

    public static SortAction<TEntity>? Action(string fieldName, bool asc)
        => _instance.TryGetAction(fieldName, asc, out var sortAction)
            ? sortAction
            : null;
    
    public static IReadOnlyList<string> Fields
        => _instance.AvailableFields;
    
    public static bool Validate(string? fieldName)
        => _instance.IsValid(fieldName);

    protected void Register(string fieldName, Expression<Func<TEntity, object?>> fieldSelector)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);
        ArgumentNullException.ThrowIfNull(fieldSelector);

        if (!_map.TryAdd(fieldName, fieldSelector))
        {
            throw new ArgumentException($"Sorting field '{fieldName}' is already registered");
        }
    }
    
    private bool TryGetAction(
        string fieldName,
        bool asc,
        [NotNullWhen(true)] out SortAction<TEntity>? sortAction)
    {
        if (!IsValid(fieldName))
        {
            sortAction = null;
            return false;
        }
        
        sortAction = asc
            ? SortAction<TEntity>.Asc(_map[fieldName])
            : SortAction<TEntity>.Desc(_map[fieldName]);
        
        return true;
    }
}