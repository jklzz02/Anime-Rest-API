namespace AnimeApi.Server.Core.Extensions;

public static class EnumerableExtensions
{
    public static bool ContainsIgnoreCase(this IEnumerable<string> list, string item)
        => list.Any(i => string.Equals(i, item, StringComparison.OrdinalIgnoreCase));
}