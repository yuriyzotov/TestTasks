using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradingServer.Data;
using TradingServer.Model;


namespace TradingServer.Tests.Generator
{
    [TestClass]
    public class QuotesSourceUnitTests
    {
        [TestMethod]
        public void QuotesSource_Should_Push_New_Ticker_Every_500_ms()
        {
            var mockModel = new Moq.Mock<IQuotesModel>();
            var tick = 0;
            var errors = 0;
            mockModel.Setup(m => m.UpdateQuote(It.IsAny<Quote>())).Callback(() =>
                {
                    if (tick != 0)
                    {
                        var elapsed = Environment.TickCount - tick;
                        if (elapsed < 450)
                            Interlocked.Increment(ref errors);
                        if (elapsed > 550)
                            Interlocked.Increment(ref errors);
                    }
                    tick = Environment.TickCount;

                });

            var source = new QuotesSource(mockModel.Object);
            
            source.Start(500);
            Thread.Sleep(3000);
            source.Stop();

            Assert.AreEqual(0, errors, "Update was elapsed in wrong timeouts");

        }
    }
}
