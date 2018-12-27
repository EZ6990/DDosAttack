using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Victim
{
    class TCPVictim
    {
        TcpListener server;
        string password;
        Structure structure;
        struct botInfo
        {
            public string IP;
            public UInt16 port;
            public double time;
        }

        private class Structure
        {
            List<botInfo> lst;

            public Structure()
            {
                this.lst = new List<botInfo>();
            }
             
            public void add(botInfo bi)
            {
              
                lst.Insert(0, bi);
                if (lst.Count == 11)
                {
                    lst.RemoveAt(10);
                }
            }

            public bool checkIfHacked()
            {
                return lst.Count == 10 && lst[0].time - lst[9].time < 1000;  
            }

        }


        public TCPVictim(string password)
        {
            structure = new Structure();
            server = new TcpListener(IPAddress.Loopback,new Random().Next(1024,65537));
            this.password = password;
            Console.WriteLine("Server listening on port {0}, password is {1}", ((IPEndPoint)(server.LocalEndpoint)).Port , this.password);

        }

        public void listen()
        {
            String data;
            botInfo bi;
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                writeData("Please enter your password\r\n",client);
                if ((data = readData(6,client)).Equals(this.password)){
                    writeData("Access granted\r\n",client);
                    bi.IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    bi.port = Convert.ToUInt16(((IPEndPoint)client.Client.RemoteEndPoint).Port);
                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    double secondsSinceEpoch = t.TotalMilliseconds;
                    bi.time = secondsSinceEpoch;
                    this.structure.add(bi);
                    data = readData(40,client);
                    if(this.structure.checkIfHacked())
                        Console.WriteLine("{0}", data);
                }
                
                client.Close();
            }
        }
        public String readData(int size,TcpClient client)
        {
            String readData = "";
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[size];
                stream.Read(buffer, 0, buffer.Length);
                readData = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                stream.Flush();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return readData;
        }
        private void writeData(string message,TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            Byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            stream.Flush();


        }



    }
}
