using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using TradingServer.Configuration;
using TradingServer.Formatters;
using TradingServer.Model;
using TradingServer.Server;
using System.Linq;
using System.Threading.Tasks;

namespace TradingServer.Tests
{
    [TestClass]
    public class ServerIntegrationTest
    {
        private StandardKernel myResolver;

        [TestInitialize]
        public void Init()
        {
            myResolver = new StandardKernel();
            myResolver.Bind<IQuoteValidator>().To<QuoteValidator>();
            myResolver.Bind<IQuotesModel>().To<QuotesTable>().InSingletonScope();
            myResolver.Bind<IQuotesSource>().To<QuotesSource>().InSingletonScope();
            myResolver.Bind<ITradingServerShell>().To<TradingServerShell>().InSingletonScope();
            myResolver.Bind<IClientSynchronisation>().To<ClientSynchronisation>().InSingletonScope();
            myResolver.Bind<IClentListener>().To<ClentListener>();

            var config = new Moq.Mock<IServerConfiguration>();
            config.Setup(c => c.ServerPort).Returns(3333);
            config.Setup(c => c.QuotesSourceFrequency).Returns(200);
            config.Setup(c => c.ClientPushingFrequency).Returns(1000);

            myResolver.Bind<IServerConfiguration>().ToConstant(config.Object);
            

        }

        [TestMethod]
        public void Server_Should_Work_With_Four_Clients()
        {
            var server = myResolver.Get<ITradingServerShell>();
            var clients = new Thread[4];
            var responses = new List<string>[4];
            bool stop = false;

            for (int i = 0; i < clients.Length;i++ )
            {
                var n = i;
                responses[i] = new List<string>();
                clients[i] = new Thread(() =>
                {
                    var client = new TcpClient( );
                
                    client.Connect("localhost", 3333);
                    var dataStream = client.GetStream();
                    using (var reader = new StreamReader(dataStream))
                    {
                        using (var writer = new StreamWriter(dataStream))
                        {
                            var data = reader.ReadLine();
                            Assert.IsTrue(2<data.Split(";".ToCharArray()).Length);
                            responses[n].Add(data);
                            writer.WriteLine("c");                            
                            do
                            {
                                Thread.Sleep(100);
                                data = reader.ReadLine();
                                responses[n].Add(data);
                            } while (!stop);
                            writer.WriteLine("q");
                        }
                    }
                });
            }


            //run server
            server.Run();
            //fill all quotes to be sure that all clients received the same list of quotes
            while (myResolver.Get<IQuotesModel>().GetQuotes(DateTime.MinValue).Count()<5)
            {
                Thread.Sleep(100);
            }

            //run clients
            foreach (var t in clients)
            {
                t.Start();
            }

            //wait till clients received
            
            Thread.Sleep(5000);

            Thread.Sleep(5000);
            stop = true;
            foreach (var t in clients)
            {
                t.Join();
            }
            server.Stop();


            for (int i = 0; i < responses[0].Count; i++)
            {
                Assert.AreEqual(responses[0][i], responses[1][i]);
                Assert.AreEqual(responses[0][i], responses[2][i]);
                Assert.AreEqual(responses[0][i], responses[3][i]);
                System.Diagnostics.Debug.WriteLine(responses[0][i]);
            }
        }
    }
}
