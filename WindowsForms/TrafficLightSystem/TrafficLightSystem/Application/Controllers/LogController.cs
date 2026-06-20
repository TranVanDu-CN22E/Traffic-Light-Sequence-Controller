using TrafficLightSystem.Domain.Models;
using TrafficLightSystem.Infrastructure.Interfaces;

namespace TrafficLightSystem.Application.Controllers
{
    public class LogController
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        public Task<List<TrafficLog>> GetLogsAsync()
        {
            return _logService.GetLogsAsync();
        }
    }
}