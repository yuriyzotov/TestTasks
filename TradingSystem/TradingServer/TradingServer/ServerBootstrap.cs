using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using TradingServer.DI;
using TradingServer.Interfaces;

namespace TradingServer
{
    class ServerBootstrap
    {
        static void Main(string[] args)
        {
            //init DI
            IKernel ninject = new StandardKernel(new TradingServerDIModule());
            var shell = ninject.Get<ITradingServerShell>();
            shell.Run();
            Console.WriteLine("Listening....Press Enter to Stop.");
            Console.ReadLine();
            shell.Stop();
            Console.WriteLine("Server Stopped. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
