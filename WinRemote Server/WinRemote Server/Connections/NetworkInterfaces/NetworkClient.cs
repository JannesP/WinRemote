using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Listener;

namespace WinRemote_Server.Connections.NetworkInterfaces
{
    abstract class NetworkClient
    {
        public abstract void Answer(NetworkInterface.Message messageId, bool message);
        public abstract void Answer(NetworkInterface.Message messageId, int message);
        public abstract void Answer(NetworkInterface.Message messageId, byte[] message);
    }
}
