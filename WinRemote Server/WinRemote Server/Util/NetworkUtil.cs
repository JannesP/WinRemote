using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRemote_Server.Connections.Listener;

namespace WinRemote_Server.Util
{
    static class NetworkUtil
    {
        public static byte[] CreateStandardTcpMessage(int messageId)
        {
            byte[] message = new byte[TcpNetworkInterface.MSG_SIZE];
            byte[] byteMessageId = Utility.CreateBytesFromInt(messageId);
            Utility.ArrayCopy(byteMessageId, ref message);
            return message;
        }

        public static byte[] CreateAdvancedTcpMessage(int messageId, byte[] data)
        {
            byte[] message = CreateStandardTcpMessage(messageId);
            Utility.ArrayCopy(data, ref message, sizeof(int));
            return message;
        }
    }
}
