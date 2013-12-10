using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextMetrics.Core.Parsers
{
    public class AlphaTokensParserStrategy : ITokensParserStrategy
    {
        private Regex myWordsRegexp;

        public AlphaTokensParserStrategy(Encoding encoding)
        {
            myWordsRegexp = new Regex(@"[\p{Ll}\p{Lu}\p{Lo}\p{Lm}\p{Lt}'-]+", RegexOptions.Compiled | RegexOptions.Singleline);
            
        }
        
        public IEnumerable<string> Parse(string line)
        {
            if(string.IsNullOrWhiteSpace(line))
                return Enumerable.Empty<string>();
            //parse sentence by regular expression
            var wordCollection = myWordsRegexp.Matches(line);
            return wordCollection.Cast<Match>().Select(s => s.ToString().Trim("'-".ToArray())).Where(s=>!string.IsNullOrWhiteSpace(s));
        }
    }
}
