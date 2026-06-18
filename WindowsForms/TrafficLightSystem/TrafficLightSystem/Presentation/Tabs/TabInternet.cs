using TrafficLightSystem.Application.Controllers;
using TrafficLightSystem.Infrastructure.Internet;
using TrafficLightSystem.Presentation.Views;

namespace TrafficLightSystem.Presentation.Tabs
{
    public partial class TabInternet : UserControl
    {
        public TabInternet()
        {
            InitializeComponent();
            LoadUI();
        }

        private void LoadUI()
        {
            var connection = new FirebaseConnection();
            var controller = new TrafficController(connection);

            var view = new TrafficControlView(controller);
            view.Dock = DockStyle.Fill;

            this.Controls.Add(view);
        }
    }
}
