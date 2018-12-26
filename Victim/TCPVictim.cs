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
        struct botInfo
        {
            public string IP;
            public UInt16 port;
            public long time;
        }

        private class structure
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
            
            server =new TcpListener(IPAddress.Loopback,0);
            this.password = password;
            Console.WriteLine("Server listening on port {0}, password is {1}", ((IPEndPoint)(server.LocalEndpoint)).Port,this.password);
        }
        public void listen()
        {


            while (true)
            {
                Console.Write("Waiting for a connection... ");
                TcpClient client = server.AcceptTcpClient();
            }

            }

    }
}
