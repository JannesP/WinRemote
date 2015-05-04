using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Listener;
using WinRemote_Server.Connections.Receiver;
using WinRemote_Server.Util;

namespace WinRemote_Server
{
    class MessageProcessor : IReceiver
    {
        public void OnListenerStatusChange(NetworkInterface.NetworkStatus status) {}

        public void OnReceiveMessage(NetworkInterface.Message message)
        {
            switch (message)
            {
                case NetworkInterface.Message.SHUTDOWN:
                    Logger.Log("DEBUG", "Shutdown requested!");
                    //WindowsHelper.Shutdown((int)WindowsHelper.ShutdownParameter.SHUTDOWN);
                    break;
            }
        }

        public void OnReceiveMessage(NetworkInterface.Message message, object extras)
        {
            throw new NotImplementedException();
        }
    }
}
