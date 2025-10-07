
using AnimeApi.Server.Core.Objects;

namespace AnimeApi.Server.Core.Extensions;
public static class ErrorExtensions
{
    public static string ToSingleLineString(this Error error)
    {
        return error.ToString().Replace("\n", " ").Replace("\r", " ");
    }

    public static string ToSingleLineString(this IEnumerable<Error> errors)
    {
        return string.Join(" | ", errors.Select(e => e.ToSingleLineString()));
    }

    public static Dictionary<string, string> TokeyValuePairs(this IEnumerable<Error> errors)
    {
        return errors.ToDictionary(e => e.Message, e => e.Details);
    }
}
