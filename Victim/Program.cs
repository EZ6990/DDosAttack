using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victim
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPVictim victim = new TCPVictim("zazapo");
            victim.listen();
        }
    }
}
