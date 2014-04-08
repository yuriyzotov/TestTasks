using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingServer.Data;
using TradingServer.Formatters;
using TradingServer.Model;

namespace TradingServer.Tests.Model
{
    [TestClass]
    public class QuotesTableUnitTests
    {

        [TestMethod]
        public void QuotesTable_Should_Return_LastUpdatedQuotes()
        {
            var mockValidator = new Moq.Mock<IQuoteValidator>();
            mockValidator.Setup(v => v.ValidateQuote(Moq.It.IsAny<Quote>())).Returns(true);
            var model = new QuotesTable(mockValidator.Object);

            model.UpdateQuote(new Quote("T1",0.1m,DateTime.Now.AddMinutes(-2)));
            model.UpdateQuote(new Quote("T2",0.2m,DateTime.Now.AddMinutes(-1)));
            model.UpdateQuote(new Quote("T3",0.3m,DateTime.Now));



            var values = model.GetQuotes(DateTime.Now.AddSeconds(-90));
            Assert.AreEqual(2, values.Count());
            Assert.IsTrue(values.Any(v=>v.Ticker=="T2"));
            Assert.IsTrue(values.Any(v => v.Ticker == "T3"));

            values = model.GetQuotes(DateTime.Now);
            Assert.AreEqual(0, values.Count());

        }
    }
}
