using TrafficLightSystem.Application.Controllers;

namespace TrafficLightSystem.Presentation.Tabs
{
    public partial class LogTab : UserControl
    {
        private readonly LogController _controller;

        public LogTab(LogController controller)
        {
            InitializeComponent();

            _controller = controller;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            await LoadLogs();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            try
            {
                await LoadLogs();
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private async Task LoadLogs()
        {
            var logs = await _controller.GetLogsAsync();

            dataGridLog.Rows.Clear();
            dataGridLog.Columns.Clear();

            dataGridLog.Columns.Add("time", "Thời gian");
            dataGridLog.Columns.Add("message", "Nội dung");

            dataGridLog.Columns["message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridLog.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridLog.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            foreach (var log in logs)
            {
                dataGridLog.Rows.Add(
                    log.timestamp?.ToDateTime().ToString("dd/MM/yyyy HH:mm:ss"),
                    log.message
                );
            }
        }
    }
}
