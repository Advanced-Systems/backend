using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Responses;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Services.HealthChecks;

public sealed class ConnectionHealthCheck : IConnectionHealthCheck, IHealthCheck
{
    private readonly ILogger<IConnectionHealthCheck> _logger;

    public ConnectionHealthCheck(ILogger<IConnectionHealthCheck> logger)
    {
        _logger = logger;
    }

    public async ValueTask<ConnectionHealthCheckResponse> GetResult()
    {
        return await Task.FromResult(new ConnectionHealthCheckResponse
        {
            IsHealthy = true,
            Description = "Hello, World!"
        });
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = await this.GetResult();

        if (result.IsHealthy) return HealthCheckResult.Healthy(result.Description);

        this._logger.LogWarning("ConnectionHealthCheck reported an unhealthy status: {Result}", result);
        return HealthCheckResult.Unhealthy(result.Description);

    }
}