using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TextMetrics.Core.Aggregators.Storage
{
    /// <summary>
    /// used to store aggregation values in external storage
    /// can be extened in future to specify other interfaces to storing
    /// </summary>
    public interface IAggregeationStorage
    {
        void SaveAggregation(ITokensAggregator aggregator);
    }
}
