namespace WinRemote_Server.Settings
{
    partial class FormSettings
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
            this.components = new System.ComponentModel.Container();
            this.button_save = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.textBox_bindingAddress = new System.Windows.Forms.TextBox();
            this.numericUpDown_port = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_bindingAddress = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).BeginInit();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(255, 57);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_close
            // 
            this.button_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_close.Location = new System.Drawing.Point(12, 57);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 23);
            this.button_close.TabIndex = 1;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // textBox_bindingAddress
            // 
            this.textBox_bindingAddress.Location = new System.Drawing.Point(99, 6);
            this.textBox_bindingAddress.Name = "textBox_bindingAddress";
            this.textBox_bindingAddress.Size = new System.Drawing.Size(100, 20);
            this.textBox_bindingAddress.TabIndex = 2;
            this.toolTip.SetToolTip(this.textBox_bindingAddress, "The binding address defines the socket to listen on.");
            this.textBox_bindingAddress.TextChanged += new System.EventHandler(this.OnSettingsChange);
            // 
            // numericUpDown_port
            // 
            this.numericUpDown_port.Location = new System.Drawing.Point(99, 32);
            this.numericUpDown_port.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_port.Name = "numericUpDown_port";
            this.numericUpDown_port.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown_port.TabIndex = 3;
            this.toolTip.SetToolTip(this.numericUpDown_port, "The Tcp port to listen on.\r\nMax: 65535");
            this.numericUpDown_port.Value = new decimal(new int[] {
            5555,
            0,
            0,
            0});
            this.numericUpDown_port.ValueChanged += new System.EventHandler(this.OnSettingsChange);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port";
            // 
            // checkBox_bindingAddress
            // 
            this.checkBox_bindingAddress.AutoSize = true;
            this.checkBox_bindingAddress.Location = new System.Drawing.Point(205, 8);
            this.checkBox_bindingAddress.Name = "checkBox_bindingAddress";
            this.checkBox_bindingAddress.Size = new System.Drawing.Size(128, 17);
            this.checkBox_bindingAddress.TabIndex = 5;
            this.checkBox_bindingAddress.Text = "Use binding address?";
            this.toolTip.SetToolTip(this.checkBox_bindingAddress, "The binding address defines the socket to listen on.");
            this.checkBox_bindingAddress.UseVisualStyleBackColor = true;
            this.checkBox_bindingAddress.CheckedChanged += new System.EventHandler(this.checkBox_bindingAddress_CheckedChanged);
            this.checkBox_bindingAddress.CheckedChanged += new System.EventHandler(this.OnSettingsChange);
            // 
            // toolTip
            // 
            this.toolTip.Tag = "The binding address defines the socket to listen on.";
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Binding Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Binding Address";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 92);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_bindingAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown_port);
            this.Controls.Add(this.textBox_bindingAddress);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_save);
            this.Name = "FormSettings";
            this.Text = "FormSettings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSettings_FormClosed);
            this.Shown += new System.EventHandler(this.FormSettings_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.TextBox textBox_bindingAddress;
        private System.Windows.Forms.NumericUpDown numericUpDown_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_bindingAddress;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label2;
    }
}