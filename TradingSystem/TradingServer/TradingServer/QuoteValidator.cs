using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Data;
using TradingServer.Interfaces;

namespace TradingServer
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
