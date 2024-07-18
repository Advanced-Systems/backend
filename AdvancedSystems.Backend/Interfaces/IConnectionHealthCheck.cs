using System.Threading.Tasks;

using AdvancedSystems.Backend.Responses;

namespace AdvancedSystems.Backend.Interfaces;

public interface IConnectionHealthCheck
{
    ValueTask<ConnectionHealthCheckResponse> GetResult();
}