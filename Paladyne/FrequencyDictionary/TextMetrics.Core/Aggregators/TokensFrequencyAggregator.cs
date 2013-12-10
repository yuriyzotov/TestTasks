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
        
        
        public void Aggregate(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;
            myTokensDictionary.AddOrUpdate(token.ToLowerInvariant(),1,(k,v)=>v+1);
        }

        public IEnumerable<KeyValuePair<string, long>> GetAggregation()
        {
            return myTokensDictionary.OrderByDescending(pair => pair.Value);
        }


        public bool IsSynchronized
        {
            get { return true; }
        }
    }
}
