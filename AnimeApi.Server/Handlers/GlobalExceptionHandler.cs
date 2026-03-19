using AnimeApi.Server.Core;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace AnimeApi.Server.Handlers;

public class GlobalExceptionHandler(IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var result = JsonConvert.SerializeObject(new
        {
            error = Constants.Remark.InternalServerError,
            details = environment.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred."
        });

        await httpContext.Response.WriteAsync(result, cancellationToken);

        return true;
    }
}