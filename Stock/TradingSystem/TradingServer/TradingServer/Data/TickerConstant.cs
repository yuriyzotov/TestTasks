using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingServer.Data
{
    public static class TickerConstant
    {

        public static List<string> Tickers
        {
            get
            {
                return new List<string>()
                {
                    "RUB_EUR","EUR_USD","USD_CHG","USD_AUD","USD_GBP"
                };
            }
        }
    }
}
