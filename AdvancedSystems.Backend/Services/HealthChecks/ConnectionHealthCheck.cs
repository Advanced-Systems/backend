using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Responses;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Services.HealthChecks;

internal class ConnectionHealthCheck(ILogger<IConnectionHealthCheck> logger) : IConnectionHealthCheck, IHealthCheck
{
    private readonly ILogger<IConnectionHealthCheck> _logger = logger;

    public async Task<ConnectionHealthCheckResponse> TestConnection()
    {
        return await Task.FromResult(new ConnectionHealthCheckResponse
        {
            IsHealthy = true,
            Description = "Hello, World!"
        });
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = await this.TestConnection();

        if (!result.IsHealthy)
        {
            this._logger.LogWarning("ConnectionHealthCheck reported an unhealthy status: {}", result);
            return HealthCheckResult.Unhealthy(result.Description);
        }

        return HealthCheckResult.Healthy(result.Description);
    }
}