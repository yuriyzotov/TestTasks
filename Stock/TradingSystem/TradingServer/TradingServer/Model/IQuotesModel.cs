using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingServer.Data;

namespace TradingServer.Model
{
    public interface IQuotesModel
    {
        /// <summary>
        /// update quote in model
        /// </summary>
        /// <param name="quoteValuePoint"></param>
        void UpdateQuote(Quote quoteValuePoint);

        /// <summary>
        /// get quotes from point of time, will be returned data with date > then fromPoint
        /// </summary>
        /// <param name="fromPoint">low limit of dates</param>
        /// <returns></returns>
        IEnumerable<Quote> GetQuotes(DateTime fromPoint);
    }
}
