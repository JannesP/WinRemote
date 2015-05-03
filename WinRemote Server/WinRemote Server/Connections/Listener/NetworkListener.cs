using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Receiver;

namespace WinRemote_Server.Connections.Listener
{
    abstract class NetworkListener
    {
        protected IReceiver receiver = null;

        protected NetworkListener(IReceiver receiver)
        {
            this.receiver = receiver;
        }


        public abstract void Start();
        public abstract void Stop();

    }
}
