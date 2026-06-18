using TrafficLightSystem.Domain.Models;

namespace TrafficLightSystem.Shared.Helpers
{
    public static class CommandBuilder
    {
        public static TrafficCommand PowerOn()
            => new TrafficCommand { Type = CommandConstants.POWER_ON };

        public static TrafficCommand PowerOff()
            => new TrafficCommand { Type = CommandConstants.POWER_OFF };

        public static TrafficCommand Normal()
            => new TrafficCommand { Type = CommandConstants.NORMAL };

        public static TrafficCommand Flash()
            => new TrafficCommand { Type = CommandConstants.FLASH };

        public static TrafficCommand Route(string route)
            => new TrafficCommand
            {
                Type = CommandConstants.ROUTE,
                Payload = route
            };

        public static TrafficCommand SetTime(int g, int y, int r)
            => new TrafficCommand
            {
                Type = CommandConstants.SET,
                Payload = $"{g},{y},{r}"
            };
    }
}