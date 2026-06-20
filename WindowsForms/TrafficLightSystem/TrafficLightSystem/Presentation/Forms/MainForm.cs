using TrafficLightSystem.Application.Controllers;
using TrafficLightSystem.Infrastructure.Internet;
using TrafficLightSystem.Presentation.Tabs;

namespace TrafficLightSystem.Presentation.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadTabs();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTabs();
        }

        // =========================
        // LOAD TAB SYSTEM
        // =========================
        private void LoadTabs()
        {
            // TAB COM
            var tabCom = new TabPage("COM");
            tabCom.Controls.Add(new TabCOM() { Dock = DockStyle.Fill });

            // TAB BLUETOOTH
            var tabBluetooth = new TabPage("Bluetooth");
            tabBluetooth.Controls.Add(new TabBluetooth() { Dock = DockStyle.Fill });

            // TAB INTERNET
            var tabInternet = new TabPage("Internet");
            tabInternet.Controls.Add(new TabInternet() { Dock = DockStyle.Fill });

            // TAB LOG
            var firestore = new FirestoreService();
            var logController = new LogController(firestore);
            var tabLogs = new TabPage("Logs");

            tabLogs.Controls.Add(
                new LogTab(logController)
                {
                    Dock = DockStyle.Fill
                });

            // ADD TO TABCONTROL
            tabControl1.TabPages.Add(tabCom);
            tabControl1.TabPages.Add(tabBluetooth);
            tabControl1.TabPages.Add(tabInternet);
            tabControl1.TabPages.Add(tabLogs);

        }
    }
}