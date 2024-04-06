using System.Threading.Tasks;

using AdvancedSystems.Backend.Responses;

namespace AdvancedSystems.Backend.Interfaces
{
    internal interface IConnectionHealthCheck
    {
        Task<ConnectionHealthCheckResponse> TestConnection();
    }
}
