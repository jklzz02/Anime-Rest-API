
using AnimeApi.Server.Core.Objects;

namespace AnimeApi.Server.Core.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Error"/> class and collections of <see cref="Error"/> objects.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Converts the specified <see cref="Error"/> object to a single-line string representation by
    /// replacing newline characters with spaces and returning the formatted string.
    /// </summary>
    /// <param name="error">The <see cref="Error"/> object to convert to a single-line string.</param>
    /// <returns>A string representation of the <see cref="Error"/> object with newline characters replaced by spaces.</returns>
    public static string ToSingleLineString(this Error error)
    {
        return error.ToString().Replace("\n", " ").Replace("\r", " ");
    }

    /// <summary>
    /// Converts a collection of <see cref="Error"/> objects to a single-line string by
    /// joining their individual single-line string representations with a separator.
    /// </summary>
    /// <param name="errors">The collection of <see cref="Error"/> objects to convert to a single-line string.</param>
    /// <returns>A single-line string representation of the collection, with each <see cref="Error"/> object separated by a pipe character ("|").</returns>
    public static string ToSingleLineString(this IEnumerable<Error> errors)
    {
        return string.Join(" | ", errors.Select(e => e.ToSingleLineString()));
    }

    /// <summary>
    /// Converts a collection of <see cref="Error"/> objects into a dictionary where the keys are the error messages
    /// and the values are the corresponding error details.
    /// </summary>
    /// <param name="errors">A collection of <see cref="Error"/> objects to be converted into key-value pairs.</param>
    /// <returns>A dictionary where each key is an error message and the corresponding value is the error details.</returns>
    public static Dictionary<string, string> ToKeyValuePairs(this IEnumerable<Error> errors)
    {
        return errors.ToDictionary(e => e.Message, e => e.Details);
    }
}
