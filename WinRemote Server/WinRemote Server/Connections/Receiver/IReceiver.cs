using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Listener;
using WinRemote_Server.Connections.NetworkInterfaces;

namespace WinRemote_Server.Connections.Receiver
{
    interface IReceiver
    {
        void OnReceiveMessage(NetworkClient connectedClient, NetworkInterface.Message messageId, byte[] data);
        void OnListenerStatusChange(NetworkInterface networkInterface, NetworkInterface.NetworkStatus status);
        object Invoke(Delegate method);
    }
}
