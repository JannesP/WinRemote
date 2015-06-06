using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinRemote_Server.Connections.Listener;
using WinRemote_Server.Connections.NetworkInterfaces;
using WinRemote_Server.Connections.Receiver;
using WinRemote_Server.Settings;
using WinRemote_Server.Util;

namespace WinRemote_Server
{
    public partial class FormMain : Form, IReceiver
    {
        public const string VERSION = "0.0.1d";

        public static RichTextBox logBox = null;

        private NetworkInterface tcpInterface;
        private MessageProcessor messageProcessor;

        public FormMain()
        {
            InitializeComponent();
            messageProcessor = new MessageProcessor();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logBox = richTextBox_log;
            Logger.Log("Information", "Program started with version number: " + VERSION);
            SettingsHelper.Init();

            string[] commandLineArgs = Environment.GetCommandLineArgs();
            ProcessCommandLineArgs(commandLineArgs);

            //setup additional listeners
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            this.Resize += new EventHandler(this.FormMain_Resize);
        }

        private void ProcessCommandLineArgs(string[] args)
        {
            if (args.Length > 0) args[0] = null;
            foreach (string arg in args)
            {
                if (arg == null) continue;
                Logger.Log("Arg", arg); //print all args before processing
                switch (arg)
                {
                    case "-autostart":   //start trayified, restart all listeners etc.
                        Trayify(false);
                        break;
                    case "-tcp":   //start with tcp on
                        SetupTcpListener();
                        tcpInterface.Start();
                        break;
                    default:
                        Logger.Log("ArgProcessing", "The Argument \"" + arg + "\" is invalid, ignoring!");
                        break;
                }
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Program.onShutdown = true;
            SettingsHelper.SaveSettings();
            if (tcpInterface != null)
            {
                tcpInterface.Stop();
            }
        }

        private void Trayify()
        {
            Trayify(true);
        }

        private void Trayify(bool showBalloonTip)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.ShowInTaskbar = false;
            if (showBalloonTip) notifyIconMain.ShowBalloonTip(1750);
        }

        private void Detrayify()
        {
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Focus();
        }

        private void SetupTcpListener()
        {
            if (tcpInterface != null && (tcpInterface.GetStatus() != NetworkInterface.NetworkStatus.CLOSED && tcpInterface.GetStatus() != NetworkInterface.NetworkStatus.CRASHED))
            {
                tcpInterface.Stop();
                Logger.Log("Information", "Waiting for TcpInterface to stop.");
                System.Threading.Timer t = new System.Threading.Timer((state) => { SetupTcpListener(); }, null, 5, -1);
                return;
            }
            IPAddress bindingAddress;
            bool isAddressValid = IPAddress.TryParse(LoadedSettings.KEY_BINDING_ADDRESS, out bindingAddress);
            if (!LoadedSettings.GetBool(LoadedSettings.KEY_USE_BINDING_ADDRESS) || !isAddressValid)
            {
                bindingAddress = IPAddress.Any;
                if (LoadedSettings.GetBool(LoadedSettings.KEY_USE_BINDING_ADDRESS) && !LoadedSettings.GetString(LoadedSettings.KEY_BINDING_ADDRESS).Equals(""))
                {
                    Logger.Log("WARNING", "The binding address is not valid. Listening on all interfaces! (0.0.0.0)");
                }
            }
            Logger.Log("Information", "Creating TcpListener on " + bindingAddress.ToString() + ":" + LoadedSettings.GetString(LoadedSettings.KEY_PORT));
            tcpInterface = new TcpNetworkInterface(bindingAddress, LoadedSettings.GetNumber(LoadedSettings.KEY_PORT), this);
            tcpInterface.AddNetworkReceiver(messageProcessor);
            tcpInterface.Name = "tcp";
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings(this);
            formSettings.Show();
        }

        private void FormMain_EnabledChanged(object sender, EventArgs e)
        {
            if (Enabled)
            {
                Logger.Log("Information", "Settings closed, reloading vars ...");
                NetworkInterface.NetworkStatus oldStatus = tcpInterface.GetStatus();
                SetupTcpListener(); //stop old listener and setup a new one with updated settings
                if (oldStatus == NetworkInterface.NetworkStatus.STARTING || oldStatus == NetworkInterface.NetworkStatus.RUNNING)    //restart the server when it was started before
                {
                    tcpInterface.Start();
                }
            }
        }

        private void button_startServer_Click(object sender, EventArgs e)
        {
            if (tcpInterface == null)
            {
                SetupTcpListener();
                tcpInterface.Start();
                return;
            }
            switch (tcpInterface.GetStatus())
            {
                case NetworkInterface.NetworkStatus.RUNNING:
                    tcpInterface.Stop();
                    break;
                case NetworkInterface.NetworkStatus.CRASHED:
                case NetworkInterface.NetworkStatus.CLOSED:
                    SetupTcpListener();
                    tcpInterface.Start();
                    break;
            }
        }
        
        void IReceiver.OnReceiveMessage(NetworkClient client, NetworkInterface.Message message, byte[] data) {   }

        void IReceiver.OnListenerStatusChange(NetworkInterface networkInterface, NetworkInterface.NetworkStatus status)
        {
            switch (status)
            {
                case NetworkInterface.NetworkStatus.STARTING:
                    textBox_serverStatus.Text = "Starting ...";
                    textBox_serverStatus.BackColor = Color.FromArgb(185, 255, 185);

                    button_startServer.Enabled = false;
                    break;
                case NetworkInterface.NetworkStatus.RUNNING:
                    textBox_serverStatus.Text = "Running";
                    textBox_serverStatus.BackColor = Color.FromArgb(117, 255, 117);

                    button_startServer.Text = "Stop";
                    button_startServer.Enabled = true;
                    break;
                case NetworkInterface.NetworkStatus.CRASHED:
                    textBox_serverStatus.Text = "CRASHED! Info in Log.";
                    textBox_serverStatus.BackColor = Color.FromArgb(255, 0, 66);

                    button_startServer.Text = "Start";
                    button_startServer.Enabled = true;
                    break;
                case NetworkInterface.NetworkStatus.CLOSING:
                    textBox_serverStatus.Text = "Closing ...";
                    textBox_serverStatus.BackColor = Color.FromArgb(255, 160, 160);

                    button_startServer.Enabled = false;
                    break;
                case NetworkInterface.NetworkStatus.CLOSED:
                    textBox_serverStatus.Text = "Closed";
                    textBox_serverStatus.BackColor = Color.FromArgb(255, 128, 128);

                    button_startServer.Text = "Start";
                    button_startServer.Enabled = true;
                    break;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Trayify();
            }
            else
            {
                Application.Exit();
            }
        }

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Detrayify();
        }

        private void contextMenuTrayIcon_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItemAudioDevice.DropDownItems.Clear();
            Dictionary<int, string> soundDevices = WindowsHelper.GetSoundDevices();
            foreach(int id in soundDevices.Keys)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = id + " - " + soundDevices[id];
                menuItem.Name = id.ToString();
                menuItem.Click += new EventHandler(OnAudioDeviceMenuItemClick);
                toolStripMenuItemAudioDevice.DropDownItems.Add(menuItem);
            }

        }

        private void OnAudioDeviceMenuItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            WindowsHelper.SetSoundDevice(int.Parse(menuItem.Name));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Trayify();
            }
        }
    }
}
