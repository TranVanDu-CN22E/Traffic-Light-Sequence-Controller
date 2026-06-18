namespace TrafficLightSystem.Presentation.Views
{
    partial class TrafficControlView
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
            groupBox1 = new GroupBox();
            lblStatus = new TextBox();
            label4 = new Label();
            btnDisconnect = new Button();
            btnConnect = new Button();
            groupBox2 = new GroupBox();
            btnFlash = new Button();
            btnNormal = new Button();
            btnPowerOff = new Button();
            btnPowerOn = new Button();
            groupBox3 = new GroupBox();
            btnUpdateTime = new Button();
            txtRed = new NumericUpDown();
            txtYellow = new NumericUpDown();
            txtGreen = new NumericUpDown();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox4 = new GroupBox();
            btnD = new Button();
            btnC = new Button();
            btnB = new Button();
            btnA = new Button();
            groupBox6 = new GroupBox();
            txtReceive = new TextBox();
            groupBox5 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtRed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtYellow).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtGreen).BeginInit();
            groupBox4.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(lblStatus);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(btnDisconnect);
            groupBox1.Controls.Add(btnConnect);
            groupBox1.Font = new Font("Segoe UI", 10F);
            groupBox1.Location = new Point(35, 38);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(922, 89);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Quản lý kết nối";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(683, 39);
            lblStatus.Name = "lblStatus";
            lblStatus.ReadOnly = true;
            lblStatus.Size = new Size(192, 30);
            lblStatus.TabIndex = 4;
            lblStatus.TextChanged += lblStatus_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(585, 42);
            label4.Name = "label4";
            label4.Size = new Size(96, 23);
            label4.TabIndex = 3;
            label4.Text = "Trạng thái: ";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(190, 36);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(150, 29);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "NGẮT KẾT NỐI";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(22, 36);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(150, 29);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "KẾT NỐI";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnFlash);
            groupBox2.Controls.Add(btnNormal);
            groupBox2.Controls.Add(btnPowerOff);
            groupBox2.Controls.Add(btnPowerOn);
            groupBox2.Font = new Font("Segoe UI", 10F);
            groupBox2.Location = new Point(35, 159);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(514, 205);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Điều khiển hệ thống";
            // 
            // btnFlash
            // 
            btnFlash.BackColor = Color.Transparent;
            btnFlash.FlatStyle = FlatStyle.Flat;
            btnFlash.ForeColor = Color.Goldenrod;
            btnFlash.Location = new Point(264, 123);
            btnFlash.Name = "btnFlash";
            btnFlash.Size = new Size(224, 48);
            btnFlash.TabIndex = 3;
            btnFlash.Text = "BẬT HỆ THỐNG";
            btnFlash.UseVisualStyleBackColor = false;
            btnFlash.Click += btnFlash_Click;
            // 
            // btnNormal
            // 
            btnNormal.BackColor = Color.Transparent;
            btnNormal.FlatStyle = FlatStyle.Flat;
            btnNormal.ForeColor = Color.DarkSlateBlue;
            btnNormal.Location = new Point(24, 123);
            btnNormal.Name = "btnNormal";
            btnNormal.Size = new Size(224, 48);
            btnNormal.TabIndex = 3;
            btnNormal.Text = "BẬT HỆ THỐNG";
            btnNormal.UseVisualStyleBackColor = false;
            btnNormal.Click += btnNormal_Click;
            // 
            // btnPowerOff
            // 
            btnPowerOff.BackColor = Color.Crimson;
            btnPowerOff.ForeColor = Color.White;
            btnPowerOff.Location = new Point(264, 47);
            btnPowerOff.Name = "btnPowerOff";
            btnPowerOff.Size = new Size(224, 48);
            btnPowerOff.TabIndex = 1;
            btnPowerOff.Text = "TẮT HỆ THỐNG";
            btnPowerOff.UseVisualStyleBackColor = false;
            btnPowerOff.Click += btnPowerOff_Click;
            // 
            // btnPowerOn
            // 
            btnPowerOn.BackColor = Color.SeaGreen;
            btnPowerOn.ForeColor = Color.White;
            btnPowerOn.Location = new Point(24, 47);
            btnPowerOn.Name = "btnPowerOn";
            btnPowerOn.Size = new Size(224, 48);
            btnPowerOn.TabIndex = 0;
            btnPowerOn.Text = "BẬT HỆ THỐNG";
            btnPowerOn.UseVisualStyleBackColor = false;
            btnPowerOn.Click += btnPowerOn_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnUpdateTime);
            groupBox3.Controls.Add(txtRed);
            groupBox3.Controls.Add(txtYellow);
            groupBox3.Controls.Add(txtGreen);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(label1);
            groupBox3.Font = new Font("Segoe UI", 10F);
            groupBox3.Location = new Point(588, 159);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(369, 205);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Cấu hình thời gian (giây)";
            // 
            // btnUpdateTime
            // 
            btnUpdateTime.BackColor = Color.FromArgb(103, 58, 183);
            btnUpdateTime.ForeColor = Color.White;
            btnUpdateTime.Location = new Point(32, 123);
            btnUpdateTime.Name = "btnUpdateTime";
            btnUpdateTime.Size = new Size(283, 48);
            btnUpdateTime.TabIndex = 6;
            btnUpdateTime.Text = "CẬP NHẬP THỜI GIAN";
            btnUpdateTime.UseVisualStyleBackColor = false;
            btnUpdateTime.Click += button1_Click;
            // 
            // txtRed
            // 
            txtRed.Location = new Point(246, 78);
            txtRed.Name = "txtRed";
            txtRed.Size = new Size(69, 30);
            txtRed.TabIndex = 5;
            txtRed.Value = new decimal(new int[] { 55, 0, 0, 0 });
            txtRed.ValueChanged += txtRed_ValueChanged;
            // 
            // txtYellow
            // 
            txtYellow.Location = new Point(138, 78);
            txtYellow.Name = "txtYellow";
            txtYellow.Size = new Size(69, 30);
            txtYellow.TabIndex = 4;
            txtYellow.Value = new decimal(new int[] { 5, 0, 0, 0 });
            txtYellow.ValueChanged += txtYellow_ValueChanged;
            // 
            // txtGreen
            // 
            txtGreen.Location = new Point(32, 78);
            txtGreen.Name = "txtGreen";
            txtGreen.Size = new Size(69, 30);
            txtGreen.TabIndex = 3;
            txtGreen.Value = new decimal(new int[] { 35, 0, 0, 0 });
            txtGreen.ValueChanged += txtGreen_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(138, 41);
            label3.Name = "label3";
            label3.Size = new Size(53, 23);
            label3.TabIndex = 2;
            label3.Text = "Vàng:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(246, 41);
            label2.Name = "label2";
            label2.Size = new Size(68, 23);
            label2.TabIndex = 1;
            label2.Text = "B Xanh:";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 41);
            label1.Name = "label1";
            label1.Size = new Size(69, 23);
            label1.TabIndex = 0;
            label1.Text = "A Xanh:";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btnD);
            groupBox4.Controls.Add(btnC);
            groupBox4.Controls.Add(btnB);
            groupBox4.Controls.Add(btnA);
            groupBox4.Font = new Font("Segoe UI", 10F);
            groupBox4.Location = new Point(588, 393);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(369, 104);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Chuyển hướng";
            // 
            // btnD
            // 
            btnD.Location = new Point(196, 35);
            btnD.Name = "btnD";
            btnD.Size = new Size(68, 49);
            btnD.TabIndex = 3;
            btnD.Text = "D";
            btnD.UseVisualStyleBackColor = true;
            btnD.Click += button5_Click;
            // 
            // btnC
            // 
            btnC.Location = new Point(281, 35);
            btnC.Name = "btnC";
            btnC.Size = new Size(69, 49);
            btnC.TabIndex = 2;
            btnC.Text = "C";
            btnC.UseVisualStyleBackColor = true;
            btnC.Click += button4_Click;
            // 
            // btnB
            // 
            btnB.Location = new Point(112, 34);
            btnB.Name = "btnB";
            btnB.Size = new Size(66, 50);
            btnB.TabIndex = 1;
            btnB.Text = "B";
            btnB.UseVisualStyleBackColor = true;
            btnB.Click += button3_Click;
            // 
            // btnA
            // 
            btnA.Location = new Point(23, 34);
            btnA.Name = "btnA";
            btnA.Size = new Size(69, 50);
            btnA.TabIndex = 0;
            btnA.Text = "A";
            btnA.UseVisualStyleBackColor = true;
            btnA.Click += button2_Click;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(txtReceive);
            groupBox6.Location = new Point(35, 393);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(514, 218);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "Phản hồi từ Arduino";
            // 
            // txtReceive
            // 
            txtReceive.BackColor = SystemColors.Control;
            txtReceive.Font = new Font("Segoe UI", 10F);
            txtReceive.Location = new Point(24, 38);
            txtReceive.Multiline = true;
            txtReceive.Name = "txtReceive";
            txtReceive.ReadOnly = true;
            txtReceive.ScrollBars = ScrollBars.Vertical;
            txtReceive.Size = new Size(475, 147);
            txtReceive.TabIndex = 4;
            // 
            // groupBox5
            // 
            groupBox5.Location = new Point(588, 519);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(369, 92);
            groupBox5.TabIndex = 6;
            groupBox5.TabStop = false;
            groupBox5.Text = "Tiện ích mở rộng";
            // 
            // TrafficControlView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(groupBox5);
            Controls.Add(groupBox6);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "TrafficControlView";
            Size = new Size(1000, 650);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtRed).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtYellow).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtGreen).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnConnect;
        private Button btnDisconnect;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Button btnPowerOn;
        private Button btnFlash;
        private Button btnNormal;
        private Button btnPowerOff;
        private Label label2;
        private Label label1;
        private NumericUpDown txtGreen;
        private Label label3;
        private NumericUpDown txtRed;
        private NumericUpDown txtYellow;
        private Button btnUpdateTime;
        private Button btnD;
        private Button btnC;
        private Button btnB;
        private Button btnA;
        private GroupBox groupBox6;
        private TextBox txtReceive;
        private GroupBox groupBox5;
        private TextBox lblStatus;
        private Label label4;
    }
}
