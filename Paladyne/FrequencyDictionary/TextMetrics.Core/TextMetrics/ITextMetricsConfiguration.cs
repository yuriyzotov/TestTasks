using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextMetrics.Core.Aggregators;
using TextMetrics.Core.Aggregators.Storage;
using TextMetrics.Core.Parsers;


namespace TextMetrics.Core.TextMetrics
{
    /// <summary>
    /// used to configure the text metrics processor
    /// can use the DI framework to create realisations
    /// </summary>
    public interface ITextMetricsConfiguration
    {
        ITextParser CreateTextParser();

        ITokensParserStrategy CreateTokenParserStrategy();

        ITokensAggregator CreateTokenAggregator();
        
        IAggregeationStorage CreateAggregationStorage();
    }
}
