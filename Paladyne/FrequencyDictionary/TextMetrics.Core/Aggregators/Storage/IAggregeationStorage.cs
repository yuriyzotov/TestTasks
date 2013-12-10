using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TextMetrics.Core.Aggregators.Storage
{
    public interface IAggregeationStorage
    {
        void SaveAggregation(ITokensAggregator aggregator);
    }
}
