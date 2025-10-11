using System.Reflection;
using AnimeApi.Server.Core.Objects;
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
    ///  A list of <see cref="Error"/> objects where the keys are JSON property names and the
    /// values are the corresponding error messages. If no <see cref="JsonPropertyAttribute"/> is found, the original property name is used as the key.
    /// </returns>
    public static List<Error> ToJsonKeyedErrors<T>(this IEnumerable<ValidationFailure> results)
        {
            if (results is null || !results.Any())
            {
                return [];
            }

            var propertyMap = typeof(T)
                    .GetProperties()
                    .Where(p => p.IsDefined(typeof(JsonPropertyAttribute), false))
                    .ToDictionary(
                        p => p.Name,
                        p => p.GetCustomAttribute<JsonPropertyAttribute>()!.PropertyName
                    );

            return results.Select(e => 
                Error.Validation(
                    propertyMap.TryGetValue(e.PropertyName, out var jsonKey) ? jsonKey : e.PropertyName,
                    e.ErrorMessage)
            )
            .ToList();
        }
}