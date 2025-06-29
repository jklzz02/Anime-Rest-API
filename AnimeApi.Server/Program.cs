using System.Text;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.DataAccess.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AnimeApi.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

       builder.Services
           .AddAuthorization()
           .AddControllers()
           .AddNewtonsoftJson();
       
       builder.Services
           .AddDataAccess(connectionString!)
           .AddMemoryCache()
           .AddBusiness()
           .AddIdentity()
           .AddAuthentication("Bearer")
           .AddJwtBearer("Bearer", options =>
           {
               var config = builder.Configuration.GetSection("Authentication:Jwt");
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidIssuer = config["Issuer"],
                   ValidateAudience = true,
                   ValidAudience = config["Audience"],
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(config["Secret"] ?? throw new Exception("JWT secret missing"))),
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.FromMinutes(2)
               };
           });

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

                    var result = JsonConvert.SerializeObject(new
                    {
                        error = "Internal server error",
                        details = app.Environment.IsDevelopment() ? ex.Message : "An unexpected error occurred."
                    });

                    await context.Response.WriteAsync(result);
                }
            });
        });


        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        
        app.UseAuthorization();
        
        app.MapControllers();
        
        app.Run();
    }
}