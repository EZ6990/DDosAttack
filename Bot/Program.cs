using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {

            UDPControl x = new UDPControl();
            new Thread(new ThreadStart(x.listen)).Start();
            Console.WriteLine("Bot is listening on port {0}", x.getPort());
            x.sendRepeatdMessage(x.getPort() + "", 10);

           
        }
    }
}
