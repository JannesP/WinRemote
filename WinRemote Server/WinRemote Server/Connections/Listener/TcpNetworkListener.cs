using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Receiver;

namespace WinRemote_Server.Connections.Listener
{
    class TcpNetworkListener : NetworkListener
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public TcpNetworkListener(int port, IReceiver receiver) : this(IPAddress.Any, port, receiver) {   }

        public TcpNetworkListener(IPAddress bindingAddress, int port, IReceiver receiver) : base(receiver)
        {
            tcpListener = new TcpListener(bindingAddress, port);
        }

        public override void Start()
        {
            listenThread = new Thread(new ThreadStart(Listen));
            listenThread.Start();
        }

        private void Listen()
        {
            try
            {
                byte[] bytes = new byte[256];
                int data = -1;
                tcpListener.Start();
                while (true)
                {
                    Logger.Log("TcpListener", "Waiting for Connection...");
                    Socket client = tcpListener.AcceptSocket();
                    client.Receive(bytes);
                    Logger.Log("TcpListener", "Connection from: " + client.RemoteEndPoint.ToString() + ".");
                    data = ReadIntFromByteArray(bytes, 0);
                    Logger.Log("TcpListener", string.Format("Received {0}", data));
                    bytes = new byte[256];
                    receiver.OnReceiveMessage(data);
                    client.Close();
                }

            }
            catch (Exception ex)
            {
                Logger.Log("Exception", ex.ToString());
                tcpListener.Stop();
            }
        }

        private static int ReadIntFromByteArray(byte[] input, int startIndex)
        {
            if (input.Length - 4 > startIndex) throw new ArgumentOutOfRangeException("The startIndex was too big.");

            int result = 0;

            for (int i = 3; i >= 0; i--)
            {
                result |= input[startIndex + i] << (8 * i);
            }

            return result;
        }

        public override void Stop()
        {
            if (listenThread != null && tcpListener != null)
            {
                listenThread.Abort();
                tcpListener.Stop();
            }
        }
    }
}
