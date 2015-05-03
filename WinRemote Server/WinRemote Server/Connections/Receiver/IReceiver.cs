using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRemote_Server.Connections.Receiver
{
    interface IReceiver
    {
        void OnReceiveMessage(int message);
        void OnReceiveMessage(int message, object extras);
    }
}
