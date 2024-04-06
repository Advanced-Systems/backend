using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Services.HealthChecks;
using AdvancedSystems.Backend.Swagger;

using Asp.Versioning;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedSystems.Backend.Core.Extensions
{
    internal static class AddMethods
    {
        internal static IServiceCollection AddBackendHealthChecks(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionHealthCheck, ConnectionHealthCheck>();

            services.AddHealthChecks()
                .AddCheck<ConnectionHealthCheck>(nameof(ConnectionHealthCheck));

            return services;
        }

        internal static IServiceCollection AddBackendDocumentation(this IServiceCollection services)
        {
            services.AddApiVersioning(option => {
                option.DefaultApiVersion = new ApiVersion(1.0);
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ReportApiVersions = true;
                option.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
            }).AddMvc().AddApiExplorer();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(option => option.OperationFilter<SwaggerDefaultValues>());

            return services;
        }
    }
}
