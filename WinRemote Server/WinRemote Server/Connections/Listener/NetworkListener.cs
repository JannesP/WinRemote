using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinRemote_Server.Connections.Receiver;

namespace WinRemote_Server.Connections.Listener
{
    abstract class NetworkListener
    {
        private NetworkStatus status = NetworkStatus.CLOSED;

        protected IReceiver receiver = null;

        protected NetworkListener(IReceiver receiver)
        {
            this.receiver = receiver;
        }

        protected void StatusChanged(NetworkStatus status)
        {
            this.status = status;
            FormMain.logBox.Invoke((MethodInvoker)(() => receiver.OnListenerStatusChange(status)));
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

        public enum NetworkMessage
        {
            SHUTDOWN
        }
    }
}
