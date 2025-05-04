using System.Reflection;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Extensions;

public static class ValidationResultExtensions
{
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