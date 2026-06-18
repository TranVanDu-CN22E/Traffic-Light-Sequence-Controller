using System;
using System.Drawing;
using System.Windows.Forms;
using TrafficLightSystem.Application.Controllers;

namespace TrafficLightSystem.Presentation.Views
{
    public partial class TrafficControlView : UserControl
    {
        private TrafficController _controller;

        public TrafficControlView(TrafficController controller)
        {
            InitializeComponent();
            _controller = controller;

            // =========================
            // SUBSCRIBE DATA RECEIVED
            // =========================
            _controller.DataReceived += OnDataReceived;
        }

        // =========================
        // RECEIVE FROM DEVICE
        // =========================
        private void OnDataReceived(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    txtReceive.AppendText(msg + Environment.NewLine);
                }));
            }
            else
            {
                txtReceive.AppendText(msg + Environment.NewLine);
            }
        }

        // =========================
        // LOAD
        // =========================
        private void TrafficControlView_Load(object sender, EventArgs e)
        {
            SetStatus("DISCONNECTED", Color.Red);
        }

        // =========================
        // STATUS UI
        // =========================
        private void SetStatus(string text, Color color)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = color;
        }

        // =========================
        // CONNECTION
        // =========================
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_controller.Connect())
                SetStatus("CONNECTED", Color.LimeGreen);
            else
                SetStatus("CONNECT FAILED", Color.Red);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _controller.Disconnect();
            SetStatus("DISCONNECTED", Color.Red);
        }

        // =========================
        // SYSTEM MODE
        // =========================
        private void btnPowerOn_Click(object sender, EventArgs e)
            => _controller.PowerOn();

        private void btnPowerOff_Click(object sender, EventArgs e)
            => _controller.PowerOff();

        private void btnNormal_Click(object sender, EventArgs e)
            => _controller.Normal();

        private void btnFlash_Click(object sender, EventArgs e)
            => _controller.Flash();

        // =========================
        // ROUTE BUTTONS
        // =========================
        private void button2_Click(object sender, EventArgs e) => ToggleRoute(btnA);
        private void button3_Click(object sender, EventArgs e) => ToggleRoute(btnB);
        private void button4_Click(object sender, EventArgs e) => ToggleRoute(btnC);
        private void button5_Click(object sender, EventArgs e) => ToggleRoute(btnD);

        private void ToggleRoute(Button btn)
        {
            bool state = btn.Tag != null && (bool)btn.Tag;
            state = !state;

            btn.Tag = state;
            btn.BackColor = state ? Color.LimeGreen : Color.LightGray;

            SendRoute();
        }

        private void SendRoute()
        {
            string route = "";

            if (IsActive(btnA)) route += "A";
            if (IsActive(btnB)) route += "B";
            if (IsActive(btnC)) route += "C";
            if (IsActive(btnD)) route += "D";

            _controller.SendRoute(route);
        }

        private bool IsActive(Button btn)
            => btn.Tag != null && (bool)btn.Tag;

        // =========================
        // UPDATE TIME
        // =========================
        private void button1_Click(object sender, EventArgs e)
        {
            _controller.SetTime(
                (int)txtGreen.Value,
                (int)txtYellow.Value,
                (int)txtRed.Value
            );
        }

        // =========================
        // UNUSED EVENTS
        // =========================
        private void txtGreen_ValueChanged(object sender, EventArgs e) { }
        private void txtYellow_ValueChanged(object sender, EventArgs e) { }
        private void txtRed_ValueChanged(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void btnReload_Click(object sender, EventArgs e) { }
        private void lblStatus_TextChanged(object sender, EventArgs e) { }
    }
}