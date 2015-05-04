﻿using System;
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
using WinRemote_Server.Connections.Receiver;
using WinRemote_Server.Settings;

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
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            messageProcessor = new MessageProcessor();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logBox = richTextBox_log;
            Logger.Log("Information", "Program started with version number: " + VERSION);
            SettingsHelper.Init();
            SetupTcpListener();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            tcpInterface.Stop();
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
                Logger.Log("Information", "Settings closed, reloading vars from settings ...");
                SetupTcpListener();
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SettingsHelper.SaveSettings();
        }

        private void button_startServer_Click(object sender, EventArgs e)
        {
            switch (tcpInterface.GetStatus())
            {
                case NetworkInterface.NetworkStatus.RUNNING:
                    tcpInterface.Stop();
                    break;
                case NetworkInterface.NetworkStatus.CRASHED:
                case NetworkInterface.NetworkStatus.CLOSED:
                    tcpInterface.Start();
                    break;
            }
        }

        void IReceiver.OnReceiveMessage(NetworkInterface.Message message)
        {
            throw new NotImplementedException();
        }

        void IReceiver.OnReceiveMessage(NetworkInterface.Message message, object extras)
        {
            throw new NotImplementedException();
        }

        void IReceiver.OnListenerStatusChange(NetworkInterface.NetworkStatus status)
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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to shut down?", "Shutdown?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                messageProcessor.OnReceiveMessage(NetworkInterface.Message.SHUTDOWN);
            }
        }
    }
}