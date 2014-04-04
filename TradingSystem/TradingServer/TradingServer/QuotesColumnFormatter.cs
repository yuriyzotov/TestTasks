using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Interfaces;

namespace TradingServer
{
    public class QuotesColumnFormatter : IQuotesFormatter
    {
        public string FormatQuotes(IEnumerable<Data.Quote> quotes)
        {
            var result = new StringBuilder();
            foreach(var q in quotes)
            {
                result.AppendFormat("{0}={1}", q.Ticker,q.Value);
                result.AppendLine();    
            }
            return result.ToString();
        }
    }
}
