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
        void OnReceiveMessage(int message);
        void OnReceiveMessage(int message, object extras);
        void OnListenerStatusChange(NetworkListener.NetworkStatus status);
    }
}
