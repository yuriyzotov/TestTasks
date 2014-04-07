using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingServer.Data
{
    public struct Quote
    {
        public Quote(string ticker, decimal value):this(ticker,value,DateTime.Now)
        {
        }
        public Quote(string ticker, decimal value, DateTime point)
        {
            Ticker = ticker;
            Value = value;
            Point = point;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Ticker);
        }

        /// <summary>
        /// name of the quote ticker
        /// </summary>
        public string Ticker;
        
        /// <summary>
        /// value of the quote
        /// </summary>
        public decimal Value;
        
        /// <summary>
        /// point in time when qiote was generated
        /// </summary>
        public DateTime Point;
    }
}
