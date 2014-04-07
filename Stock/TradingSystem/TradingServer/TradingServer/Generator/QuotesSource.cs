using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingServer.Data;
using TradingServer.Model;


namespace TradingServer
{
    public class QuotesSource : IQuotesSource
    {
        public QuotesSource(IQuotesModel model)
        {
            this.myModel = model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">in milliceconds</param>
        public void Start(int timeout)
        {
            myStoping = false;
            myWorker = new Thread(GenerateQuote);
            myWorker.Start(timeout);
            
        }

        public void Stop()
        {
            myStoping = true;
        }

        /// <summary>
        /// async operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GenerateQuote(object obj)
        {
            var tickers = TickerConstant.Tickers;
            
            var timeout = (int) obj;
            var r = new Random();
            while (!myStoping)
            {
                Thread.Sleep(timeout);
                decimal value = decimal.Round(new decimal(r.NextDouble()) * 5.0m, 2, MidpointRounding.AwayFromZero);
                var tickerName = tickers.ElementAt(r.Next(tickers.Count));

                myModel.UpdateQuote(new Quote(tickerName, value,DateTime.Now));
            }
        }

        private bool myStoping;
        private IQuotesModel myModel;
        private Thread myWorker;
    }
}
