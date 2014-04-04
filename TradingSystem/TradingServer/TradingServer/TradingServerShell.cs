using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Interfaces;

namespace TradingServer
{
    public class TradingServerShell : ITradingServerShell
    {
        private IQuotesSource mySource;
        private IQuotesModel myModel;
        private TcpListener myListener;
        
        public TradingServerShell(IQuotesSource source, IQuotesModel model)
        {
            mySource = source;
            myModel = model;
        }




        public async void Run()
        {
            mySource.Start(600);

            myListener = new TcpListener(GetEndpoint());
            Console.Out.WriteLine("Start listening on {0}", myListener.LocalEndpoint.ToString());
            myListener.Start();

            try
            {
                while (!myStopping)
                {
                    var tcpClient = await myListener.AcceptTcpClientAsync();
                    HandleConnectionAsync(tcpClient);
                }
            }
            catch (Exception exp)
            {
                Console.Out.WriteLine(exp.Message);
            }

        }

        private async void HandleConnectionAsync(TcpClient tcpClient)
        {
            string clientInfo = tcpClient.Client.RemoteEndPoint.ToString();
            Console.Out.WriteLine("Got connection request from {0}", clientInfo);
            IQuotesFormatter formatter = new QuotesRowFormatter();
            try
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    using (var reader = new StreamReader(networkStream))
                    {
                        using (var writer = new StreamWriter(networkStream))
                        {
                            writer.AutoFlush = true;
                            await writer.WriteLineAsync(formatter.FormatQuotes(myModel.GetQuotes(DateTime.MinValue)));
                            var lastPoolDate = DateTime.Now;
                            var task = reader.ReadLineAsync();
                            while (true)
                            {

                                int timeout = 2000;
                                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                                {
                                    // task completed within timeout
                                    var dataFromServer = task.Result;
                                    Console.Out.WriteLine(dataFromServer);

                                    if (dataFromServer == "r")
                                    {
                                        formatter = new QuotesRowFormatter();
                                    }
                                    else if (dataFromServer == "c")
                                    {
                                        formatter = new QuotesColumnFormatter();
                                    }
                                    else if (dataFromServer == "q")
                                    {
                                        //exit
                                        break;
                                    }
                                    task = reader.ReadLineAsync();
                                }
                                else
                                {
                                    // timeout logic
                                    var data = formatter.FormatQuotes(myModel.GetQuotes(lastPoolDate));
                                    if(!string.IsNullOrEmpty(data))
                                    {
                                        await writer.WriteLineAsync(data);
                                        lastPoolDate = DateTime.Now;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.Out.WriteLine(exp.Message);
            }
            finally
            {
                Console.Out.WriteLine("Closing the client connection - {0}",
                            clientInfo);
                tcpClient.Close();
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
