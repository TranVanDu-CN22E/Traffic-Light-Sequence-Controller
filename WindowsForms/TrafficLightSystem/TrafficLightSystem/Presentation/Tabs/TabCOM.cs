using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficLightSystem.Application.Controllers;
using TrafficLightSystem.Presentation.Views;
using TrafficLightSystem.Infrastructure.Interfaces;
using TrafficLightSystem.Infrastructure.COM;

namespace TrafficLightSystem.Presentation.Tabs
{
    public partial class TabCOM : UserControl
    {
        public TabCOM()
        {
            InitializeComponent();
            LoadUI();
        }

        private void LoadUI()
        {
            var connection = new TcpConnection(); // COM giả TCP
            var controller = new TrafficController(connection);

            var view = new TrafficControlView(controller);
            view.Dock = DockStyle.Fill;

            this.Controls.Add(view);
        }
    }
}
