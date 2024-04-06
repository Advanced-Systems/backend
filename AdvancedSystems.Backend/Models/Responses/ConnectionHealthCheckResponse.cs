namespace AdvancedSystems.Backend.Responses
{
    public record ConnectionHealthCheckResponse
    {
        public required bool IsHealthy { get; init; }

        public required string Description { get; init; }
    }
}
