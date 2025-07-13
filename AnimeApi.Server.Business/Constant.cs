namespace AnimeApi.Server.Business;

public static class Constant
{
    public const string App = "AnimeApi";

    public static class Authentication
    {
        public const string DefaultScheme = "Bearer";
    }
    
    public static class UserAccess
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

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