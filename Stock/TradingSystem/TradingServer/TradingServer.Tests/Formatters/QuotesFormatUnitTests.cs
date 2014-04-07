using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingServer.Data;
using TradingServer.Formatters;

namespace TradingServer.Tests.Formatters
{
    [TestClass]
    public class QuotesFormatUnitTests
    {

        private readonly Quote[] data = new Quote[] { 
            new Quote(TickerConstant.Tickers[0], 1.0m),
            new Quote(TickerConstant.Tickers[1], 1.1m),
            new Quote(TickerConstant.Tickers[2], 1.2m),
            new Quote(TickerConstant.Tickers[3], 1.3m),
            new Quote(TickerConstant.Tickers[4], 1.4m),
        };


        [TestMethod]
        public void QuotesRowFormatter_Should_Return_Quotes_in_Row()
        {
            var nf = Thread.CurrentThread.CurrentCulture.NumberFormat.Clone() as NumberFormatInfo;
            nf.NumberDecimalSeparator = ".";
            var expected = string.Format("{0}={1};{2}={3};{4}={5};{6}={7};{8}={9}",
                data[0].Ticker, data[0].Value.ToString("F2",nf),
                data[1].Ticker, data[1].Value.ToString("F2",nf),
                data[2].Ticker, data[2].Value.ToString("F2", nf),
                data[3].Ticker, data[3].Value.ToString("F2", nf),
                data[4].Ticker, data[4].Value.ToString("F2", nf));
            
            var formatter = new QuotesRowFormatter();
            var result = formatter.FormatQuotes(data);

            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void QuotesColumnFormatter_Should_Return_Quotes_in_Column()
        {
            var nf = Thread.CurrentThread.CurrentCulture.NumberFormat.Clone() as NumberFormatInfo;
            nf.NumberDecimalSeparator = ".";
            var expected = string.Format("{1}={2}{0}{3}={4}{0}{5}={6}{0}{7}={8}{0}{9}={10}{0}",
                Environment.NewLine,
                data[0].Ticker, data[0].Value.ToString("F2", nf),
                data[1].Ticker, data[1].Value.ToString("F2", nf),
                data[2].Ticker, data[2].Value.ToString("F2", nf),
                data[3].Ticker, data[3].Value.ToString("F2", nf),
                data[4].Ticker, data[4].Value.ToString("F2", nf));

            var formatter = new QuotesColumnFormatter();
            var result = formatter.FormatQuotes(data);

            Assert.AreEqual(expected, result);

        }
    }
}
