using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextMetrics.Core.Parsers
{
    /// <summary>
    /// used to parse one string by tokens (for example by words)
    /// </summary>
    public interface ITokensParserStrategy
    {
        IEnumerable<string> Parse(string line);
    }
}
