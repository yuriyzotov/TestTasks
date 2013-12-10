using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextMetrics.Core.Parsers
{

    /// <summary>
    /// parse one text line  and extract words from there (aplpha symbols and - and ')
    /// </summary>
    public class AlphaTokensParserStrategy : ITokensParserStrategy
    {
        
        private char[] myTrimmedSymbols = "'-".ToArray();
        private Regex myWordsRegexp;

        public AlphaTokensParserStrategy(Encoding encoding)
        {
            //setup parsing expression
            myWordsRegexp = new Regex(@"[\p{Ll}\p{Lu}\p{Lo}\p{Lm}\p{Lt}'-]+", RegexOptions.Compiled | RegexOptions.Singleline);
        }

        /// <summary>
        /// Parse line to extract words
        /// </summary>
        /// <param name="line">line to parse</param>
        /// <returns>Return enumerable collection of words in the line</returns>
        public IEnumerable<string> Parse(string line)
        {
            if(string.IsNullOrWhiteSpace(line))
                return Enumerable.Empty<string>();

            //parse sentence by regular expression
            var wordCollection = myWordsRegexp.Matches(line);
            return wordCollection.Cast<Match>().Select(s => s.ToString().Trim(myTrimmedSymbols)).Where(s=>!string.IsNullOrWhiteSpace(s));
        }
    }
}
