using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Listener;

namespace WinRemote_Server.Connections.Receiver
{
    interface IReceiver
    {
        void OnReceiveMessage(NetworkInterface.Message message);
        void OnReceiveMessage(NetworkInterface.Message message, object extras);
        void OnListenerStatusChange(NetworkInterface.NetworkStatus status);
    }
}
