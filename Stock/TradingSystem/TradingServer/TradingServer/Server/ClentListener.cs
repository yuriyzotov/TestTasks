using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Interfaces;

namespace TradingServer.Server
{
    public class ClentListener : TradingServer.Server.IClentListener
    {
        private IQuotesModel myModel;
        private IClientSynchronisation mySync;

        public ClentListener(IQuotesModel model, IClientSynchronisation sync)
        {
            this.myModel = model;
            this.mySync = sync;
        }

        public async void HandleConnectionAsync(TcpClient tcpClient)
        {
            string clientInfo = tcpClient.Client.RemoteEndPoint.ToString();
            Console.Out.WriteLine("Got connection request from {0}", clientInfo);
            
            try
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    using (var reader = new StreamReader(networkStream))
                    {
                        using (var writer = new StreamWriter(networkStream))
                        {
                            await RequestsHandler(reader, writer);
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

        private async Task RequestsHandler(StreamReader reader, StreamWriter writer)
        {
            writer.AutoFlush = true;

            IQuotesFormatter formatter = new QuotesRowFormatter();
            
            //get all quotes
            await writer.WriteLineAsync(formatter.FormatQuotes(myModel.GetQuotes(DateTime.MinValue)));
            
            //setup last pool date for client
            var lastPoolDate = DateTime.Now;
            var readerTask = reader.ReadLineAsync();

            while (true)
            {

                int timeout = 2000;
                if (await Task.WhenAny(readerTask, this.mySync.PushNotification) == readerTask)
                {
                    // task completed within timeout
                    var dataFromServer = readerTask.Result;
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
                    readerTask = reader.ReadLineAsync();
                }
                else
                {
                    // timeout logic
                    var data = formatter.FormatQuotes(myModel.GetQuotes(lastPoolDate));
                    if (!string.IsNullOrEmpty(data))
                    {
                        await writer.WriteLineAsync(data);
                        lastPoolDate = DateTime.Now;
                    }

                }
            }
        }

        
    }
}
