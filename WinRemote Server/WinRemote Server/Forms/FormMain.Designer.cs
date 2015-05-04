namespace WinRemote_Server
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_startServer = new System.Windows.Forms.Button();
            this.textBox_serverStatus = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.button_settings = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_startServer
            // 
            this.button_startServer.Location = new System.Drawing.Point(226, 12);
            this.button_startServer.Name = "button_startServer";
            this.button_startServer.Size = new System.Drawing.Size(79, 23);
            this.button_startServer.TabIndex = 0;
            this.button_startServer.Text = "Start";
            this.button_startServer.UseVisualStyleBackColor = true;
            this.button_startServer.Click += new System.EventHandler(this.button_startServer_Click);
            // 
            // textBox_serverStatus
            // 
            this.textBox_serverStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.textBox_serverStatus.Enabled = false;
            this.textBox_serverStatus.Location = new System.Drawing.Point(104, 15);
            this.textBox_serverStatus.Name = "textBox_serverStatus";
            this.textBox_serverStatus.ReadOnly = true;
            this.textBox_serverStatus.Size = new System.Drawing.Size(116, 20);
            this.textBox_serverStatus.TabIndex = 1;
            this.textBox_serverStatus.Text = "Stopped";
            this.textBox_serverStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tcp server status:";
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox_log.Location = new System.Drawing.Point(0, 125);
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.ReadOnly = true;
            this.richTextBox_log.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox_log.Size = new System.Drawing.Size(510, 151);
            this.richTextBox_log.TabIndex = 3;
            this.richTextBox_log.Text = "";
            // 
            // button_settings
            // 
            this.button_settings.Location = new System.Drawing.Point(12, 33);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(75, 23);
            this.button_settings.TabIndex = 4;
            this.button_settings.Text = "Settings";
            this.button_settings.UseVisualStyleBackColor = true;
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(423, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Shutdown";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 276);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.richTextBox_log);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_serverStatus);
            this.Controls.Add(this.button_startServer);
            this.Name = "FormMain";
            this.Text = "WinRemote Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.EnabledChanged += new System.EventHandler(this.FormMain_EnabledChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_startServer;
        private System.Windows.Forms.TextBox textBox_serverStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.Button button_settings;
        private System.Windows.Forms.Button button1;
    }
}

