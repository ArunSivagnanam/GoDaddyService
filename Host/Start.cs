using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Host
{
    class Start
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(GoDaddyChatService.ChatService));
            host.Open();

            Console.Write("Service up and running...");
            Console.Read();
        }
    }
}
