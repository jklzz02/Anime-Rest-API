namespace AnimeApi.Server.Core;

/// <summary>
/// Provides a centralized location for defining constant values used throughout the application.
/// </summary>
public static class Constants
{
    public const string App = "AnimeApi";

    /// <summary>
    /// Contains constants related to authentication mechanisms and configurations within the application.
    /// </summary>
    public static class Authentication
    {
        public const string DefaultScheme = "Bearer";
    }

    public static class Cors
    {
        public const string ClientPolicy = "allow-client";
    }

    public static class Pagination
    {
        public const int MaxPageSize = 100;
        public const int MinPageSize = 10;
    }

    /// <summary>
    /// Represents a set of predefined user access roles within the application.
    /// </summary>
    public static class UserAccess
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    /// <summary>
    /// Contains predefined string constants representing common HTTP status remarks.
    /// These remarks are used to describe the outcome of specific operations or responses within the application.
    /// </summary>
    public static class Remark
    {
        public const string InternalServerError = "Internal Server Error";
        public const string NotFound = "Not Found";
        public const string Unauthorized = "Unauthorized";
        public const string Forbidden = "Forbidden";
        public const string BadRequest = "Bad Request";
        public const string Conflict = "Conflict";
        public const string Ok = "Ok";
        public const string Created = "Created";
        public const string NoContent = "No Content";
    }

    /// <summary>
    /// Contains constants related to the caching mechanisms used within the application.
    /// </summary>
    public static class Cache
    {
        public const int DefaultExpirationMinutes = 5;
        public const int CacheSize = 2048;
        public const int MaxCachedItemSize = 256;
        public const int DefaultCachedItemSize = 128;
        public const int MinCachedItemSize = 64;
    }
}