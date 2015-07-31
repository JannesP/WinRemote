using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinRemote_Server.Connections.NetworkInterfaces;
using WinRemote_Server.Connections.Receiver;
using WinRemote_Server.Settings;

namespace WinRemote_Server.Connections.Listener
{
    class TcpNetworkInterface : NetworkInterface
    {
        public const int MSG_SIZE = 32;

        // Thread signal. 
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        private TcpListener tcpListener;
        private Thread listenThread;

        private bool listen = false;

        public TcpNetworkInterface(int port, IReceiver receiver) : this(IPAddress.Any, port, receiver) {   }

        public TcpNetworkInterface(IPAddress bindingAddress, int port, IReceiver receiver) : base(receiver)
        {
            tcpListener = new TcpListener(bindingAddress, port);
        }

        public override void Start()
        {
            LoadedSettings.WriteValue(LoadedSettings.KEY_TCP_RUNNING, true);
            StatusChanged(NetworkStatus.STARTING);
            listen = true;
            listenThread = new Thread(() => Listen(tcpListener));
            listenThread.Start();
        }

        private void Listen(TcpListener tcpListener)
        {
            try
            {
                tcpListener.Start();
                StatusChanged(NetworkStatus.RUNNING);
                while (listen)
                {
                    // Set the event to nonsignaled state.
                    tcpClientConnected.Reset();

                    // Start to listen for connections from a client.
                    Logger.Log("TcpListener", "Waiting for a connection...");

                    // Accept the connection.  
                    // BeginAcceptSocket() creates the accepted socket.
                    tcpListener.BeginAcceptSocket(new AsyncCallback(DoAcceptTcpClientCallback), tcpListener);

                    // Wait until a connection is made and processed before  
                    // continuing.
                    tcpClientConnected.WaitOne();
                }
                StatusChanged(NetworkStatus.CLOSED);
            }
            catch (Exception ex)
            {
                Logger.Log("Exception", ex.ToString());
                base.StatusChanged(NetworkStatus.CRASHED);
            }
            finally
            {
                tcpListener.Stop();
                Logger.Log("TcpListener", "Server closed!");
            }
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            if (!listen) return;
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;
            try {
                // End the operation and display the received data on  
                // the console.
                using (Socket client = listener.EndAcceptSocket(ar))
                {
                    // Process the connection here.
                    byte[] buffer = new byte[MSG_SIZE];
                    int messageId = -1;
                    client.Receive(buffer);
                    Logger.Log("TcpListener", "Connection from: " + client.RemoteEndPoint.ToString() + ", processing ...");
                    messageId = Util.Utility.ReadIntFromByteArray(buffer, 0);
                    Logger.Log("TcpListener", string.Format("Received id: {0}.", messageId));
                    byte[] extractedData = new byte[buffer.Length - 4];
                    Util.Utility.ArrayCopy(buffer, 4, ref extractedData, 0, extractedData.Length);
                    Logger.Log("TcpListener", string.Format("Id: {0} had the following data: {1}.", messageId, Util.Utility.ArrayToReadableString(extractedData)));
                    base.MessageReceived(new TcpNetworkClient(client), messageId, extractedData);
                    Logger.Log("TcpListener", "Client connection completed");
                }
            }
            catch (ObjectDisposedException)
            {
                Logger.Log("TcpListener", "TcpListener stopped! (stream disposed)");
            }
            // Signal the calling thread to continue.
            tcpClientConnected.Set();

        }

        public override void Stop()
        {
            if (!Program.onShutdown) LoadedSettings.WriteValue(LoadedSettings.KEY_TCP_RUNNING, false); //don't set the tcpListener on off if we are shutting down the application
            if (GetStatus() == NetworkStatus.RUNNING || GetStatus() == NetworkStatus.STARTING || GetStatus() == NetworkStatus.CLOSING)
            {
                StatusChanged(NetworkStatus.CLOSING);
            }
            listen = false;
            tcpClientConnected.Set();
            
        }
    }
}
