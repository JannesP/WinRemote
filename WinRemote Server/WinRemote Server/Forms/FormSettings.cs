using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRemote_Server.Settings
{
    public partial class FormSettings : Form
    {
        private Form parent;
        public FormSettings(Form parent)
        {
            this.parent = parent;
            InitializeComponent();

            numericUpDown_port.Value = LoadedSettings.GetNumber(LoadedSettings.KEY_PORT);
            checkBox_bindingAddress.Checked = LoadedSettings.GetBool(LoadedSettings.KEY_USE_BINDING_ADDRESS);
            textBox_bindingAddress.Text = LoadedSettings.GetString(LoadedSettings.KEY_BINDING_ADDRESS);

            textBox_bindingAddress.Enabled = checkBox_bindingAddress.Checked;
            
            button_save.Enabled = false;
        }

        private void FormSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Enabled = true;
            parent.Activate();
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {
            parent.Enabled = false;
            Activate();
            Logger.Log("Information", "Settings opened.");
        }

        private void checkBox_bindingAddress_CheckedChanged(object sender, EventArgs e)
        {
            textBox_bindingAddress.Enabled = checkBox_bindingAddress.Checked;
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            LoadedSettings.WriteValue(LoadedSettings.KEY_PORT, numericUpDown_port.Value.ToString());
            LoadedSettings.WriteValue(LoadedSettings.KEY_USE_BINDING_ADDRESS, checkBox_bindingAddress.Checked.ToString());
            LoadedSettings.WriteValue(LoadedSettings.KEY_BINDING_ADDRESS, textBox_bindingAddress.Text);

            SettingsHelper.SaveSettings();

            Close();
        }

        private void OnSettingsChange(object sender, EventArgs e)
        {
            button_save.Enabled = true;
        }
    }
}
