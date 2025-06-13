using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.DataAccess.Extensions;
using Microsoft.AspNetCore.Diagnostics;

namespace AnimeApi.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
       builder.Services.AddAuthorization();
       
       var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

       builder.Services
           .AddControllers()
           .AddNewtonsoftJson();
       
       builder.Services
           .AddMemoryCache()
           .AddDataAccess(connectionString!)
           .AddBusiness();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (errorFeature != null)
                {
                    var ex = errorFeature.Error;

                    var result = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        error = "Internal server error",
                        details = app.Environment.IsDevelopment() ? ex.Message : "An unexpected error occurred."
                    });

                    await context.Response.WriteAsync(result);
                }
            });
        });


        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();
        
        app.Run();
    }
}