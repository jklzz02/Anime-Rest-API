using System.Text;

namespace AnimeApi.Server.Business.Extensions;

public static class StringFormatter
{
    public static string ToTitleCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return $"{char.ToUpper(str[0])}{str[1..].ToLower()}";
    }
}