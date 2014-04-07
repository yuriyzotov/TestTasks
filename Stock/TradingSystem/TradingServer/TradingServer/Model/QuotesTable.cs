using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Data;
using TradingServer.Interfaces;

namespace TradingServer
{
    public class QuotesTable: IQuotesModel
    {
        private ConcurrentDictionary<string, Quote> myQuotesStorage = new ConcurrentDictionary<string,Quote>();
        private IQuoteValidator myValidator;

        public QuotesTable(IQuoteValidator validator)
        {
            myValidator = validator;
        }
        public void UpdateQuote(Quote quoteValuePoint)
        {
            if (myValidator.ValidateQuote(quoteValuePoint))
            {
                myQuotesStorage.AddOrUpdate(quoteValuePoint.Ticker, quoteValuePoint, (key, q) => quoteValuePoint);
            }
        }

        public IEnumerable<Quote> GetQuotes(DateTime fromPoint)
        {
            return  myQuotesStorage.Values.Where(q => q.Point >= fromPoint).ToList();
        }
    }
}
