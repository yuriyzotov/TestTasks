using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextMetrics.Core.TextMetrics
{
    /// <summary>
    /// specific implementation of text metrics calculation algorithm 
    ///     /// </summary>
    public class TextMetricsCalculator
    {
        private ITextMetricsConfiguration myConfiguration;

        /// <summary>
        /// use configuration to assemble dendensies
        /// </summary>
        /// <param name="configuration"></param>
        public TextMetricsCalculator(ITextMetricsConfiguration configuration)
        {
            myConfiguration = configuration;
        }
        
        /// <summary>
        /// calculate metrics
        /// </summary>
        /// <param name="useParallelComputing"></param>
        public void Calculate(bool useParallelComputing)
        {
            var aggregator = myConfiguration.CreateTokenAggregator();
            var parsingStrategy = myConfiguration.CreateTokenParserStrategy();

            //setup parsing-aggregation delegate
            Action<string> tokenParsingAction =
                (line) =>
                {
                    foreach (var token in parsingStrategy.Parse(line))
                    {
                        aggregator.Aggregate(token);
                    }
                };

            //check if it is possible to use parallel computing
            if (useParallelComputing && aggregator.IsSynchronized)
            {
                myConfiguration.CreateTextParser().Parse().AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).ForAll(tokenParsingAction);
            }
            else
            {
                foreach (var line in myConfiguration.CreateTextParser().Parse())
                {
                    tokenParsingAction(line);
                }
            }
            //save results
            myConfiguration.CreateAggregationStorage().SaveAggregation(aggregator);
        }
    }
}
