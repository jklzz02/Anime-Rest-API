using System.Text;
using System.Threading.RateLimiting;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Exceptions;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.DataAccess.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        ConfigurationException.ThrowIfEmpty(builder.Configuration, "ConnectionStrings:DefaultConnection");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authorization:XClientKey");
        ConfigurationException.ThrowIfMissing(builder.Configuration, "Authentication:Google");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authentication:Google:ClientId");
        ConfigurationException.ThrowIfMissing(builder.Configuration,"Authentication:Facebook");
        ConfigurationException.ThrowIfMissing(builder.Configuration, "Authentication:Discord");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authentication:Discord:ClientId");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authentication:Discord:ClientSecret");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authentication:Facebook:AppId");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authentication:Jwt:Audience");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authentication:Jwt:Issuer");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authentication:Jwt:Secret");
        ConfigurationException.ThrowIfEmpty(builder.Configuration, "RateLimiter:Enabled");
        
        var connectionString = builder.Configuration
            .GetConnectionString("DefaultConnection");

        var clientDomain = builder.Configuration
            .GetSection("Authorization")
            .GetValue<string>("ClientDomain");
        
        var clientKey = builder.Configuration
            .GetSection("Authorization")
            .GetValue<string>("XClientKey");
        
        var rateLimiterEnabled = builder.Configuration
            .GetValue<bool>("RateLimiter:Enabled");

        if (rateLimiterEnabled)
        {
            ConfigurationException.ThrowIfEmpty(builder.Configuration, "Authorization:ClientDomain");
            ConfigurationException.ThrowIfEmpty(builder.Configuration, "RateLimiter:MaxRequestsPerMinute");
            ConfigurationException.ThrowIfEmpty(builder.Configuration, "RateLimiter:QueueLimit");
        }

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(Constants.Cors.ClientPolicy,
                policy =>
                {
                    policy.WithOrigins(clientDomain!)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        builder.Services
            .AddAuthorization(options =>
            {
                options.AddPolicy(Constants.UserAccess.Admin, policy => policy.RequireRole(Constants.UserAccess.Admin));
            })
            .AddHttpClient()
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.MaxDepth = Constants.SerializerMaxDepth;
            });

        builder.Services
            .AddMappers()
            .AddDataAccess(connectionString!)
            .AddBusiness()
            .AddIdentity()
            .AddMemoryCache(options =>
            {
                options.SizeLimit = Constants.Cache.CacheSize;
            })
            .AddAuthentication(Constants.Auth.DefaultScheme)
            .AddJwtBearer(Constants.Auth.DefaultScheme, options =>
            {
                var config = builder.Configuration.GetSection("Authentication:Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config["Issuer"],
                    ValidAudience = config["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Secret"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (string.IsNullOrEmpty(context.Token))
                        {
                            context.Token = context.Request.Cookies[Constants.Auth.AccessTokenCookieName];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = Constants.App, Version = "v1" });

            c.AddSecurityDefinition(Constants.Auth.DefaultScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = Constants.Auth.DefaultScheme,
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
                            Id = Constants.Auth.DefaultScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        builder.Services.AddRateLimiter(options =>
        {
            var config = builder.Configuration.GetSection("RateLimiter");
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var origin = context.Request.Headers.Origin.ToString();
                var requestClientKey = context.Request.Headers["X-client-key"].ToString();
                
                if (origin == clientDomain && clientKey == requestClientKey)
                {
                    return RateLimitPartition.GetNoLimiter(clientDomain);
                }
                
                if (context.User?.IsInRole(Constants.UserAccess.Admin) == true)
                {
                    return RateLimitPartition.GetNoLimiter(Constants.UserAccess.Admin);
                }

                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ip,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = config.GetValue<int>("MaxRequestsPerMinute"),
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = config.GetValue<int>("QueueLimit"),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
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
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (errorFeature != null)
                {
                    var ex = errorFeature.Error;

                    var result = JsonConvert.SerializeObject(new
                    {
                        error = Constants.Remark.InternalServerError,
                        details = app.Environment.IsDevelopment() 
                            ? ex.Message
                            : "An unexpected error occurred."
                    });

                    await context.Response.WriteAsync(result);
                }
            });
        });

        app.UseHttpsRedirection();

        app.UseCors(Constants.Cors.ClientPolicy);

        app.UseAuthentication();

        if (rateLimiterEnabled)
        {
            app.UseRateLimiter();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
