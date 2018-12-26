using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    class TCPControl
    {

        TcpClient client;
        private string password;
        private string cNcName;

        public TCPControl(String server, int port, string password, string cNcName)
        {
            client = new TcpClient(server, port);
            this.password = password;
            this.cNcName = cNcName;
        }
        public void hack()
        {
            try
            {
                String recievedData;
                recievedData = readData();
                if (recievedData.Equals("Please enter your password\\r\\n"))
                {
                    writeData(this.password);
                    if ((recievedData = readData()).Equals("Access granted\\r\\n"))
                        writeData("Hacked by " + this.cNcName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void writeData(string message)
        {
            NetworkStream stream = client.GetStream();
            Byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            stream.Flush();


        }

        public String readData()
        {
            String readData = "";

            try
            {

                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[256];
                int bytesRead;
                List<Byte> dataFlow = new List<byte>();
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) == buffer.Length)
                {
                    for (int i = 0; i < bytesRead; i++)
                        dataFlow.Add(buffer[i]);
                }
                for (int i = 0; i < bytesRead; i++)
                    dataFlow.Add(buffer[i]);
                readData = Encoding.ASCII.GetString(dataFlow.ToArray(), 0, dataFlow.Count);
                stream.Flush();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return readData;
        }
    }
}
