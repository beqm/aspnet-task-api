using MediatR;
using Serilog;
using System.Text;
using FluentValidation;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Persistence;
using Serilog.Sinks.Grafana.Loki;
using Application.Common.Mappings;
using Application.Common.Behaviors;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Versioning;
using Application.Commands.Task.CreateTask;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static WebApplicationBuilder Logger(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        Serilog.Debugging.SelfLog.Enable(Console.Error);
        var lokiUrl = configuration["Loki:Url"] ?? "";
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown";
        var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u4}] {Message}{NewLine}{Exception}";

        var labels = new List<LokiLabel>
        {
            new LokiLabel { Key = "app", Value = "aspnet-task-api" },
            new LokiLabel { Key = "env", Value = environmentName }
        };

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.GrafanaLoki(
                uri: lokiUrl,
                labels: labels
            )
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }

    public static WebApplicationBuilder Swagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Type 'Bearer {token}'."
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }

    public static WebApplicationBuilder Mediatr(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        builder.Services.AddValidatorsFromAssembly(typeof(CreateTaskCommand).Assembly);
        return builder;
    }

    public static WebApplicationBuilder Repositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        return builder;
    }

    public static WebApplicationBuilder Services(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        return builder;
    }

    public static WebApplicationBuilder Database(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        return builder;
    }

    public static WebApplicationBuilder AutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        return builder;
    }

    public static WebApplicationBuilder Authentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
            };
        });

        builder.Services.AddAuthorization();

        return builder;
    }

    public static WebApplicationBuilder Versioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader("api-version")
            );
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return builder;
    }

}