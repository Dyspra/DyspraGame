using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new UDPClient();
            client.Initialize(IPAddress.Loopback, UDPServer.PORT);
            Console.WriteLine("Client initialisé vers l'IP : " + IPAddress.Loopback);
            /*client.StartMessageLoop();
            Console.WriteLine("Loop commencée !");*/
            await client.Send(Encoding.UTF8.GetBytes("Hello!"));
            Console.WriteLine("Message envoyé!");
            await client.Recieve();
        }
    }
}
