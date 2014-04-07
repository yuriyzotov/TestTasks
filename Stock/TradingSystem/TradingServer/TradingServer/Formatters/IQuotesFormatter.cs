﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingServer.Data;

namespace TradingServer.Formatters
{
    public interface IQuotesFormatter
    {
        string FormatQuotes(IEnumerable<Quote> quotes);
    }
}