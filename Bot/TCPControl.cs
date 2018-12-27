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
                recievedData = readData("Please enter your password\r\n".Length);
                if (recievedData.Equals("Please enter your password\r\n"))
                {
                    writeData(this.password);
                    if ((recievedData = readData("Access granted\r\n".Length)).Equals("Access granted\r\n"))
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

        public String readData(int size)
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
    }
}
