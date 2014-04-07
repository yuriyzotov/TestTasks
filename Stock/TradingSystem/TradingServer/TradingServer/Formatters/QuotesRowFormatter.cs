using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TradingServer.Formatters
{
    public class QuotesRowFormatter : IQuotesFormatter
    {
        public string FormatQuotes(IEnumerable<Data.Quote> quotes)
        {
            var nf = Thread.CurrentThread.CurrentCulture.NumberFormat.Clone() as NumberFormatInfo;
            nf.NumberDecimalSeparator = ".";

            var result = new StringBuilder();
            foreach(var q in quotes)
            {
                if (result.Length > 0)
                {
                    result.Append(';');
                }
                result.AppendFormat("{0}={1}", q.Ticker, q.Value.ToString("F2",nf));
            }
            return result.ToString();
        }
    }
}
