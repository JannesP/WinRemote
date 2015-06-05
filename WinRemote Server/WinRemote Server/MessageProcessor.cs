using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Listener;
using WinRemote_Server.Connections.NetworkInterfaces;
using WinRemote_Server.Connections.Receiver;
using WinRemote_Server.Util;

namespace WinRemote_Server
{
    class MessageProcessor : IReceiver
    {
        public void OnListenerStatusChange(NetworkInterface networkInterface, NetworkInterface.NetworkStatus status) {}

        public void OnReceiveMessage(NetworkClient connectedClient, NetworkInterface.Message message)
        {
            switch (message)
            {
                case NetworkInterface.Message.TEST:
                    Logger.Log("DEBUG", "Received test message.");
                    break;
                case NetworkInterface.Message.SHUTDOWN:
                    Logger.Log("DEBUG", "Shutdown requested ...");
                    Logger.Log("DEBUG", "... shutting down!"); 
                    WindowsHelper.Shutdown((int)WindowsHelper.ShutdownParameter.SHUTDOWN);
                    break;
                case NetworkInterface.Message.REQUEST_VOLUME:
                    connectedClient.Answer(message, WindowsHelper.GetSystemVolume());
                    break;
                case NetworkInterface.Message.REQUEST_MUTED:
                    connectedClient.Answer(message, WindowsHelper.GetSystemMuted());
                    break;
            }
        }

        public void OnReceiveMessage(NetworkClient connectedClient, NetworkInterface.Message message, object extras)
        {
            throw new NotImplementedException();
        }
    }
}
