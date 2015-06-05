using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Listener;
using WinRemote_Server.Util;

namespace WinRemote_Server.Connections.NetworkInterfaces
{
    class TcpNetworkClient : NetworkClient
    {
        private Socket socket;

        public TcpNetworkClient(Socket socket)
        {
            this.socket = socket;
        }

        public override void Answer(NetworkInterface.Message messageId, int message)
        {
            byte[] answer = Utility.CreateBytesFromInt(message);
            Answer(messageId, answer);
        }

        public override void Answer(NetworkInterface.Message messageId, byte[] message)
        {
            byte[] answer = NetworkUtil.CreateAdvancedTcpMessage((int)messageId, message);
            Logger.Log("TcpAnswer", "Anwering: " + arrayString(answer));
            socket.Send(answer, 0, answer.Length, SocketFlags.None);

        }

        private String arrayString(byte[] array)
        {
            String res = "";
            foreach (byte b in array)
            {
                res += ((int)b).ToString() + ", ";
            }
            return res;
        }

        ~TcpNetworkClient() {
            socket.Close();
        }
    }
}
