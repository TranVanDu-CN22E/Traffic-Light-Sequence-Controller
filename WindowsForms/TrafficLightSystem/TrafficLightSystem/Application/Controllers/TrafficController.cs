using TrafficLightSystem.Domain.Models;
using TrafficLightSystem.Infrastructure.Interfaces;
using TrafficLightSystem.Shared.Helpers;

namespace TrafficLightSystem.Application.Controllers
{
    public class TrafficController
    {
        private readonly IConnection _connection;

        public event Action<string> DataReceived;

        public TrafficController(IConnection connection)
        {
            _connection = connection;
            _connection.DataReceived += data => DataReceived?.Invoke(data);
        }

        public bool Connect() => _connection.Connect();

        public bool Disconnect() => _connection.Disconnect();

        // =========================
        // DOMAIN COMMANDS
        // =========================

        public void PowerOn()
            => Send(CommandBuilder.PowerOn());

        public void PowerOff()
            => Send(CommandBuilder.PowerOff());

        public void Normal()
            => Send(CommandBuilder.Normal());

        public void Flash()
            => Send(CommandBuilder.Flash());

        public void SendRoute(string route)
            => Send(CommandBuilder.Route(route));

        public void SetTime(int g, int y, int r)
            => Send(CommandBuilder.SetTime(g, y, r));

        // =========================
        // CORE
        // =========================
        private void Send(TrafficCommand command)
        {
            _connection.Send(command.ToString());
        }
    }
}