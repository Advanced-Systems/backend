using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Abstractions.Interfaces;
using AdvancedSystems.Backend.Abstractions.Models.Responses;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Services;

public sealed class ConnectionHealthCheck : IConnectionHealthCheck, IHealthCheck
{
    private readonly ILogger<IConnectionHealthCheck> _logger;

    public ConnectionHealthCheck(ILogger<IConnectionHealthCheck> logger)
    {
        this._logger = logger;
    }

    #region Public Methods

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
        var result = await GetResult();

        if (result.IsHealthy) return HealthCheckResult.Healthy(result.Description);

        _logger.LogWarning("ConnectionHealthCheck reported an unhealthy status: {Result}", result);
        return HealthCheckResult.Unhealthy(result.Description);

    }

    #endregion
}