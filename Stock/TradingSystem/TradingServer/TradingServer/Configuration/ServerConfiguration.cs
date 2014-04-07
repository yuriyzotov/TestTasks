using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingServer.Configuration
{
    public class ServerConfiguration : IServerConfiguration
    {
        public ServerConfiguration(string[] args)
        {
            ServerPort = 3333;
            if (args.Length > 0)
            {
                int port = 3333;
                if (int.TryParse(args[0], out port))
                {
                    ServerPort = port;
                }
            }
        }
        public int ServerPort
        {
            get;
            set;
        }

        public int QuotesSourceFrequency
        {
            get { return 600; }
        }

        public int ClientPushingFrequency
        {
            get { return 2000; }
        }
    }
}
