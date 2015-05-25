using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinRemote_Server.Connections.Receiver;

namespace WinRemote_Server.Connections.Listener
{
    abstract class NetworkInterface
    {
        private NetworkStatus status = NetworkStatus.CLOSED;

        protected IReceiver[] receivers = new IReceiver[0];

        protected NetworkInterface(IReceiver receiver)
        {
            AddNetworkReceiver(receiver);
        }

        public void AddNetworkReceiver(IReceiver receiver)
        {
            IReceiver[] newList = new IReceiver[this.receivers.Length + 1];
            for (int i = 0; i < this.receivers.Length; i++)  //Copy old listeners
            {
                newList[i] = this.receivers[i];
            }
            newList[this.receivers.Length] = receiver;
            this.receivers = newList;
        }

        protected void MessageReceived(int message)
        {
            Message msg = (Message)message;
            foreach (IReceiver receiver in receivers)
            {
                FormMain.logBox.Invoke((MethodInvoker)(() => receiver.OnReceiveMessage(msg)));
            }
        }

        protected void StatusChanged(NetworkStatus status)
        {
            this.status = status;
            foreach (IReceiver receiver in receivers)
            {
                try
                {
                    FormMain.logBox.Invoke((MethodInvoker)(() => receiver.OnListenerStatusChange(status)));
                }
                catch
                {
                    break;
                }
            }
        }

        public NetworkStatus GetStatus()
        {
            return status;
        }

        public abstract void Start();
        public abstract void Stop();

        public enum NetworkStatus
        {
            RUNNING, STARTING, CRASHED, CLOSED, CLOSING
        }

        public enum Message
        {
            TEST = 123456789,
            SHUTDOWN = 1
        }
    }
}
