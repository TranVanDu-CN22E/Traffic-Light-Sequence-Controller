using TrafficLightSystem.Domain.Models;

namespace TrafficLightSystem.Infrastructure.Interfaces
{
    public interface ILogService
    {
        Task<List<TrafficLog>> GetLogsAsync();
    }
}