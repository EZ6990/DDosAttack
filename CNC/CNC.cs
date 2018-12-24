using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CNC
{
    class CNC
    {
        private UdpClient listener;
        string IP;
        UInt16 port;
        String password;
        public CNC()
        {
            listener = new UdpClient(31337);
        }

        public void listen()
        {
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                UInt16 botPort;
                Byte[] receiveBytes = listener.Receive(ref RemoteIpEndPoint);
                if (receiveBytes.Length == 16)
                {
                    botPort = BitConverter.ToUInt16(receiveBytes, 0);
                }
            }

        }
    }
}
