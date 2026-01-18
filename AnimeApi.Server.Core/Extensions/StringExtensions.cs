
namespace AnimeApi.Server.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Converts a string to title case
    /// </summary>
    /// <param name="str">The input string to be converted.</param>
    /// <returns>A new string in title case format.</returns>
    public static string ToTitleCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return $"{char.ToUpper(str[0])}{str[1..].ToLower()}";
    }

    /// <summary>
    /// Extracts the username from an email.
    /// </summary>
    /// <param name="email">The email address to convert.</param>
    /// <returns>A username derived from the email address in title case format.</returns>
    public static string EmailToUsername(this string email)
    {
        return string.Join(" ", email.Split('@')[0].Split('.')).ToTitleCase();
    }

    /// <summary>
    /// Compares two strings for equality, ignoring case.
    /// </summary>
    /// <param name="str">The first string to compare.</param>
    /// <param name="other">The second string to compare against.</param>
    /// <returns>True if the strings are equal, ignoring case; otherwise, false.</returns>
    public static bool EqualsIgnoreCase(this string str, string other)
    {
        return string.Equals(str.Trim(), other.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Normalizes a string by removing symbols, white spaces and punctuation.
    /// </summary>
    /// <param name="str">The string to normalize.</param>
    /// <returns>The normalized string in lower case.</returns>
    public static string ToLowerNormalized(this string str)
    {
        return str
            .NormalizeString()
            .ToLowerInvariant();
    }
    
    /// <summary>
    /// Normalizes a string by removing symbols, white spaces and punctuation.
    /// </summary>
    /// <param name="str">The string to normalize.</param>
    /// <returns>The normalized string in upper case.</returns>
    public static string ToUpperNormalized(this string str)
    {
        return str
            .NormalizeString()
            .ToUpperInvariant();
    }

    /// <summary>
    /// Normalizes a string by removing symbols, white spaces, and punctuation.
    /// </summary>
    /// <param name="str">The string to be normalized.</param>
    /// <returns>The normalized string after removing unwanted characters.</returns>
    private static string NormalizeString(this string? str)
        => string.IsNullOrWhiteSpace(str)
            ? string.Empty 
            : string.Concat(str.Where(c => !char.IsWhiteSpace(c)));
}