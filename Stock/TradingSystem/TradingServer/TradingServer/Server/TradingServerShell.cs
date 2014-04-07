using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Configuration;
using TradingServer.Model;
using TradingServer.Server;

namespace TradingServer
{
    public class TradingServerShell : ITradingServerShell
    {
        private IQuotesSource mySource;
        private IQuotesModel myModel;
        private TcpListener myListener;
        private IServerConfiguration myConfig;



        [Ninject.Inject]
        public IClentListener ClientListener {get;set;}

        
        public TradingServerShell(IQuotesSource source, IQuotesModel model, IServerConfiguration config)
        {
            mySource = source;
            myModel = model;
            myConfig = config;
        }




        public async void Run()
        {
            mySource.Start(myConfig.QuotesSourceFrequency);

            myListener = new TcpListener(GetEndpoint());
            Console.Out.WriteLine("Start listening on {0}", myListener.LocalEndpoint.ToString());
            myListener.Start();

            try
            {
                while (!myStopping)
                {
                    var tcpClient = await myListener.AcceptTcpClientAsync();
                    if(ClientListener!=null)
                    {
                        ClientListener.HandleConnectionAsync(tcpClient);
                    }
                }
            }
            catch (Exception exp)
            {
                Console.Out.WriteLine(exp.Message);
            }
        }

        

        public async void Stop()
        {
            await Console.Out.WriteLineAsync("Stopping...");
            myStopping = true;
            mySource.Stop();
            myListener.Stop();
        }



        private IPEndPoint GetEndpoint()
        {
            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            //var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //var ipAddress = ipHostInfo.AddressList[0];
            return new IPEndPoint(IPAddress.Any, 3333);
        }


        public bool myStopping { get; set; }
    }
}
