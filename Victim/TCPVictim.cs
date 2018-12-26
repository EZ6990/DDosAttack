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
                return lst.Count == 10 && lst[9].time - lst[0].time < 1000;  
            }

        }


        public TCPVictim(string password)
        {
            structure = new Structure();
            server =new TcpListener(IPAddress.Loopback,12566);
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
                writeData("Please enter your password\\r\\n",client);
                if ((data = readData(client)).Equals(this.password)){
                    writeData("Access granted\\r\\n",client);
                    bi.IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    bi.port = Convert.ToUInt16(((IPEndPoint)client.Client.RemoteEndPoint).Port);
                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    double secondsSinceEpoch = t.TotalMilliseconds;
                    bi.time = secondsSinceEpoch;
                    this.structure.add(bi);
                    data = readData(client);
                    if(this.structure.checkIfHacked())
                        Console.WriteLine("{0}", data);
                  

                }
                
                client.Close();



            }
        }

        public String readData(TcpClient client)
        {
            String readData = "";

            try
            {
               
                NetworkStream stream = client.GetStream();


                byte[] buffer = new byte[2048];
                int bytesRead;
                List<Byte> dataFlow = new List<byte>();
                while (stream.DataAvailable && (bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                        dataFlow.Add(buffer[i]);
                }
                readData = Encoding.ASCII.GetString(dataFlow.ToArray(), 0, dataFlow.Count);
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
