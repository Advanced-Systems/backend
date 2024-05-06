using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

using AdvancedSystems.Backend.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AdvancedSystems.Backend.Core.Extensions;

internal static class MapMethods
{
    internal static IEndpointConventionBuilder MapConnectionHealthCheck(this IEndpointRouteBuilder endpoints, IConnectionHealthCheck healthCheck)
    {
        return endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
        {
            AllowCachingResponses = true,
            ResponseWriter = async (context, report) =>
            {
                var result = await healthCheck.TestConnection();
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
        });
    }
}