using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextMetrics.Core.TextMetrics
{
    public class TextMetricsCalculator
    {
        private ITextMetricsConfiguration myConfiguration;

        public TextMetricsCalculator(ITextMetricsConfiguration configuration)
        {
            myConfiguration = configuration;
        }
        
        public void Calculate(bool useParallelComputing)
        {
            var aggregator = myConfiguration.CreateTokenAggregator();
            var parsingStrategy = myConfiguration.CreateTokenParserStrategy();

            //setup parsing delegate
            Action<string> tokenParsingAction = 
                (line) =>  parsingStrategy.Parse(line).All(
                    token => { aggregator.Aggregate(token); return true; }
             );

            if (useParallelComputing && aggregator.IsSynchronized)
            {
                myConfiguration.CreateTextParser().Parse().AsParallel().ForAll(tokenParsingAction);
            }
            else
            {
                foreach (var line in myConfiguration.CreateTextParser().Parse())
                {
                    tokenParsingAction(line);
                }
            }
            //create output file
            myConfiguration.CreateAggregationStorage().SaveAggregation(aggregator);
        }
    }
}
