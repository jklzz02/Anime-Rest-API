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

    /// <summary>
    /// Represents a set of predefined user access roles within the application.
    /// </summary>
    public static class UserAccess
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    /// <summary>
    /// Defines a collection of status codes representing HTTP response statuses.
    /// </summary>
    public static class StatusCode
    {
        public const int Ok = 200;
        public const int Created = 201;
        public const int NoContent = 204;
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int Conflict = 409;
        public const int InternalServerError = 500;
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
}