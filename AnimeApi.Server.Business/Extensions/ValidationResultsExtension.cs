using System.Reflection;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Extensions;

public static class ValidationResultExtensions
{
    /// <summary>
    /// Converts a collection of <see cref="ValidationFailure"/> objects into a dictionary
    /// where the keys are JSON property names and the values are the corresponding error messages.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated, which contains properties decorated with <see cref="JsonPropertyAttribute"/>.</typeparam>
    /// <param name="results">The collection of <see cref="ValidationFailure"/> objects containing validation errors.</param>
    /// <returns>
    /// A dictionary where keys are the JSON property names (as specified by <see cref="JsonPropertyAttribute"/>) and
    /// values are the corresponding error messages. If no <see cref="JsonPropertyAttribute"/> is found, the original property name is used as the key.
    /// </returns>
    public static Dictionary<string, string> ToJsonKeyedErrors<T>(this IEnumerable<ValidationFailure> results)
        {
            var propertyMap = typeof(T)
                .GetProperties()
                .Where(p => p.IsDefined(typeof(JsonPropertyAttribute), false))
                .ToDictionary(
                    p => p.Name,
                    p => p.GetCustomAttribute<JsonPropertyAttribute>()!.PropertyName
                );
            
            return results?.ToDictionary(
                e => propertyMap.TryGetValue(e.PropertyName, out var jsonKey) ? jsonKey : e.PropertyName,
                e => e.ErrorMessage
            );
        }
}