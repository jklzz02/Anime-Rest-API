namespace AnimeApi.Server.Core;

/// <summary>
/// Provides a centralized location for defining constant values used throughout the application.
/// </summary>
public static class Constants
{
    /// <summary> Represents the name of the application. </summary>
    public const string App = "AnimeApi";
    
    public const int SerializerMaxDepth = 64;
    public const int MaxTextQueryLength = 60;
    public const int DefaultRetrieveCount = 12;

    /// <summary>
    /// Contains constants related to authentication mechanisms and configurations within the application.
    /// </summary>
    public static class Auth
    {
        public const string DefaultScheme = "Bearer";
        public const int RefreshTokenExpirationDays = 15;
        public const int AccessTokenExpirationMinutes = 15;
        public const string RefreshTokenCookieName = "refresh_token";
        public const string AccessTokenCookieName = "access_token";

        /// <summary>
        /// Contains constants related to accepted identity providers.
        /// </summary>
        public enum IdentityProvider
        {
            Google,
            Facebook,
            Discord
        }
    }

    /// <summary>
    /// Contains constants related to CORS policies used throughout the application.
    /// </summary>
    public static class Cors
    {
        public const string ClientPolicy = "allow-client";
    }

    /// <summary>
    /// Contains constants related to pagination settings used throughout the application.
    /// </summary>
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
        /// <summary>
        /// Represents the "Admin" user access role within the application,
        /// indicating administrative privileges for managing and accessing
        /// restricted resources or functionalities.
        /// </summary>
        public const string Admin = "Admin";
        
        /// <summary>
        /// Represents the "User" user access role within the application,
        /// indicating standard user privileges for accessing restricted resources or functionalities.
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// Defines the roles associated with user access levels in the application.
        /// </summary>
        public enum Roles
        {
            Admin = 1,
            User = 2,
        }
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

    public static class Ratings
    {
        /// <summary>General Audience - Suitable for all ages</summary>
        public const string General = "G";

        /// <summary>Parental Guidance - Some material may not be suitable for children</summary>
        public const string ParentalGuidance = "PG";

        /// <summary>Teens 13 or older - May contain mild violence, language, or suggestive themes</summary>
        public const string Teens = "PG-13";

        /// <summary>Restricted - 17+ (violence & profanity)</summary>
        public const string Restricted = "R";

        /// <summary>Restricted Plus - Mature with mild nudity</summary>
        public const string Mature = "R+";

        /// <summary>Explicit Adult Content - Hentai</summary>
        public const string AdultContent = "Rx";

        /// <summary>Unrated or not classified</summary>
        public const string Unrated = "";
    }

    /// <summary>
    /// Contains constants used for specifying sorting options in queries.
    /// </summary>
    public static class OrderBy
    {
        /// <summary>
        /// Constants representing the fields by which data can be ordered.
        /// </summary>
        public static class Fields
        {
            public const string Id = "id";
            public const string Name = "title";
            public const string ReleaseYear = "year";
            public const string ReleaseDate = "release_date";
            public const string Score = "score";
            public const string Episodes = "episodes";

            public static readonly IReadOnlyList<string> ValidFields =
            [
                Id,
                Name,
                ReleaseYear,
                ReleaseDate,
                Score,
                Episodes
            ];
        }

        /// <summary>
        /// Constants representing the direction of sorting.
        /// </summary>
        public static class StringDirections
        {
            public const string Ascending = "asc";
            public const string Descending = "desc";

            public static readonly IReadOnlyList<string> Directions = 
            [
                Ascending,
                Descending
            ]; 
        }
    }
}