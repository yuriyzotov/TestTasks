using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextMetrics.Core.Aggregators
{
    /// <summary>
    /// specify aggreagtor  frequency of single lowercase words 
    /// </summary>
    public class TokensFrequencyAggregator: ITokensAggregator
    {
        /// <summary>
        /// should be thread safely
        /// </summary>
        private ConcurrentDictionary<string, long> myTokensDictionary = new ConcurrentDictionary<string, long>();
        
        
        /// <summary>
        /// just calculate amount of words case insensitive
        /// </summary>
        /// <param name="token"></param>
        public void Aggregate(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;
            myTokensDictionary.AddOrUpdate(token.ToLowerInvariant(),1,(k,v)=>v+1);
        }
        
        /// <summary>
        /// return ordered enumeration of the word frequency
        /// order descending by frequencies figures
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, long>> GetAggregation()
        {
            return myTokensDictionary.OrderByDescending(pair => pair.Value);
        }

        /// <summary>
        /// this aggregator is synchronized
        /// </summary>
        public bool IsSynchronized
        {
            get { return true; }
        }
    }
}
