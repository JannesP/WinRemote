﻿using System;
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
        // Thread signal. 
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        private TcpListener tcpListener;
        private Thread listenThread;

        private bool listen = false;

        public TcpNetworkListener(int port, IReceiver receiver) : this(IPAddress.Any, port, receiver) {   }

        public TcpNetworkListener(IPAddress bindingAddress, int port, IReceiver receiver) : base(receiver)
        {
            tcpListener = new TcpListener(bindingAddress, port);
        }

        public override void Start()
        {
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
                Logger.Log("TcpListener", "Server closed successfully!");
            }
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            if (!listen) return;
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on  
            // the console.
            using (Socket client = listener.EndAcceptSocket(ar))
            {
                // Process the connection here.
                byte[] buffer = new byte[64];
                int data = -1;
                client.Receive(buffer);
                Logger.Log("TcpListener", "Connection from: " + client.RemoteEndPoint.ToString() + ".");
                data = ReadIntFromByteArray(buffer, 0);
                Logger.Log("TcpListener", string.Format("Received {0}", data));
                buffer = new byte[256];
                base.receiver.OnReceiveMessage(data);
            }
            Console.WriteLine("Client connected completed");

            // Signal the calling thread to continue.
            tcpClientConnected.Set();

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
            StatusChanged(NetworkStatus.CLOSING);
            listen = false;
            tcpClientConnected.Set();
        }

        
    }
}