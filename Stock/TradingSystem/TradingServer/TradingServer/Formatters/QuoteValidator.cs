using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Data;


namespace TradingServer.Formatters
{
    public class QuoteValidator : IQuoteValidator
 
    {
        public bool ValidateQuote(Data.Quote quote)
        {
            //just check that ticker name is valid
            return TickerConstant.Tickers.Contains(quote.Ticker);
        }
    }
}
