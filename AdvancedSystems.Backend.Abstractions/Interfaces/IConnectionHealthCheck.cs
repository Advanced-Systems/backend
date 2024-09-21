using System.Threading.Tasks;

using AdvancedSystems.Backend.Abstractions.Models.Responses;

namespace AdvancedSystems.Backend.Abstractions.Interfaces;

public interface IConnectionHealthCheck
{
    ValueTask<ConnectionHealthCheckResponse> GetResult();
}