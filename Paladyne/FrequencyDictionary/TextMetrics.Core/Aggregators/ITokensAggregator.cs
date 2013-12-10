using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextMetrics.Core.Aggregators
{
    /// <summary>
    /// used to aggregate of tokens by some aggregation function
    /// then can return result as an enumeration of key values pairs
    /// </summary>
    public interface ITokensAggregator
    {
        /// <summary>
        /// aggregate one exact token
        /// </summary>
        /// <param name="token">token to aggregate</param>
        void Aggregate(string token);

        /// <summary>
        /// return enumeration of aggregationa values in with keys
        /// for example can count amount of words 
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<string,long>> GetAggregation();

        /// <summary>
        /// specify if aggreagtor is thread safetly
        /// </summary>
        bool IsSynchronized { get; }
    }
}
