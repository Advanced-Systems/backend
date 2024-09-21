namespace AdvancedSystems.Backend.Abstractions.Models.Responses;

public readonly record struct ConnectionHealthCheckResponse
{
    public required bool IsHealthy { get; init; }

    public required string Description { get; init; }
}