using System;
using System.Net.Mime;

using AdvancedSystems.Backend.Core.Validators;
using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Models.Settings;
using AdvancedSystems.Backend.Services;
using AdvancedSystems.Backend.Services.HealthChecks;
using AdvancedSystems.Backend.Swagger;
using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

using Asp.Versioning;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedSystems.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackendSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AppSettings>()
                .Bind(configuration.GetRequiredSection(nameof(AppSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddSingleton<IValidateOptions<AppSettings>, AppSettingsValidator>();

        return services;
    }

    public static IServiceCollection AddBackendServices(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddGlobalExceptionHandler();
        services.AddCachingService(environment);

        return services;
    }

    #region Services

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
                ctx.ProblemDetails.Extensions.Remove("exception");
            };
        });

        return services;
    }

    public static IServiceCollection AddCachingService(this IServiceCollection services, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            // Should only be used in single server scenarios as this cache stores items in memory and doesn't
            // expand across multiple machines. For those scenarios it is recommended to use a proper distributed
            // cache that can expand across multiple machines.
            services.AddDistributedMemoryCache();
        }
        else
        {
            // See also: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0#distributed-redis-cache
            throw new NotImplementedException("TODO: Enable Redis Caching");
        }

        services.TryAdd(ServiceDescriptor.Singleton<ICachingService, CachingService>());

        return services;
    }

    #endregion

    #region Authentication and Authorization

    public static IServiceCollection AddJwtAuth(this IServiceCollection services)
    {
        var authBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        authBuilder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // TODO: configure token validation parameters properly
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("secret")),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = "issuer",
                ValidAudience = "audience",
                ValidateIssuer = true,
                ValidateAudience = true,
            };
        });

        services.AddJwtPolicy();

        return services;
    }

    public static IServiceCollection AddJwtPolicy(this IServiceCollection services)
    {
        return services.AddAuthorization(options =>
        {
            options.AddPolicy(Roles.Admin, policy =>
            {
                policy.RequireClaim(CustomClaim.IsAdmin, "true");
            });
        });
    }

    #endregion

    #region Health Checks

    public static IServiceCollection AddBackendHealthChecks(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionHealthCheck, ConnectionHealthCheck>();

        var healthCheckBuilder = services.AddHealthChecks();
        
        healthCheckBuilder
            .AddCheck<ConnectionHealthCheck>(nameof(ConnectionHealthCheck));

        return services;
    }
    
    // TODO: Make constraint type more versatile, see also: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-interfaces 
    public static IEndpointConventionBuilder MapConnectionHealthCheck<T>(this IEndpointRouteBuilder endpoints) where T : IConnectionHealthCheck
    {
        var healthCheck = endpoints.ServiceProvider.GetRequiredService<T>();
        
        return endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
        {
            AllowCachingResponses = true,
            ResponseWriter = async (context, _) =>
            {
                var result = await healthCheck.GetResult();
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(result);
            }
        });
    }
    
    #endregion

    #region Swagger

    public static IServiceCollection AddBackendDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection(nameof(AppSettings)).Get<AppSettings>();

        services.AddApiVersioning(option => {
            option.DefaultApiVersion = new ApiVersion(settings!.DefaultApiVersion);
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.ReportApiVersions = true;
            option.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        }).AddMvc().AddApiExplorer();

        services.TryAdd(ServiceDescriptor.Transient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>());
        services.AddSwaggerGen(option => option.OperationFilter<SwaggerDefaultValues>());

        return services;
    }

    #endregion
}