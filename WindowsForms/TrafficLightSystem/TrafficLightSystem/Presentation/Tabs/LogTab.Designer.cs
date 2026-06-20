namespace TrafficLightSystem.Presentation.Tabs
{
    partial class LogTab
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridLog = new DataGridView();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridLog).BeginInit();
            SuspendLayout();
            // 
            // dataGridLog
            // 
            dataGridLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridLog.Location = new Point(47, 75);
            dataGridLog.Name = "dataGridLog";
            dataGridLog.RowHeadersWidth = 51;
            dataGridLog.Size = new Size(903, 535);
            dataGridLog.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(47, 30);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(106, 29);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "TẢI LẠI";
            btnRefresh.UseMnemonic = false;
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += this.btnRefresh_Click;
            // 
            // LogTab
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(btnRefresh);
            Controls.Add(dataGridLog);
            Name = "LogTab";
            Size = new Size(1000, 650);
            ((System.ComponentModel.ISupportInitialize)dataGridLog).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridLog;
        private Button btnRefresh;
    }
}
