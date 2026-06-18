using TrafficLightSystem.Application.Controllers;
using TrafficLightSystem.Infrastructure.Bluetooth;
using TrafficLightSystem.Presentation.Views;

namespace TrafficLightSystem.Presentation.Tabs
{
    public partial class TabBluetooth : UserControl
    {
        public TabBluetooth()
        {
            InitializeComponent();
            LoadUI();
        }

        private void LoadUI()
        {
            string port = "COM4"; // outing

            var connection = new BluetoothConnection(port);
            var controller = new TrafficController(connection);

            var view = new TrafficControlView(controller);
            view.Dock = DockStyle.Fill;

            this.Controls.Add(view);
        }
    }
}
