using System.Text;
using AnimeApi.Server.Business;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core;
using AnimeApi.Server.DataAccess.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AnimeApi.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

       builder.Services
           .AddAuthorization(options =>
           {
               options.AddPolicy(Constants.UserAccess.Admin, policy => policy.RequireRole(Constants.UserAccess.Admin));
           })
           .AddControllers()
           .AddNewtonsoftJson();

       builder.Services
           .AddDataAccess(connectionString!)
           .AddMemoryCache(options => 
           {
              options.SizeLimit = Constants.Cache.DefaultCachedItemSize;
           })
           .AddBusiness()
           .AddIdentity()
           .AddAuthentication(Constants.Authentication.DefaultScheme)
           .AddJwtBearer(Constants.Authentication.DefaultScheme, options =>
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
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = Constants.App, Version = "v1" });

            c.AddSecurityDefinition(Constants.Authentication.DefaultScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = Constants.Authentication.DefaultScheme,
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = Constants.Authentication.DefaultScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

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
                context.Response.StatusCode = Constants.StatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (errorFeature != null)
                {
                    var ex = errorFeature.Error;

                    var result = JsonConvert.SerializeObject(new
                    {
                        error = Constants.Remark.InternalServerError,
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