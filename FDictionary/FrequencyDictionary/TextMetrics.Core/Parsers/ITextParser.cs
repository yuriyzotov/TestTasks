using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextMetrics.Core.Parsers
{
    /// <summary>
    /// used to parse text and return enumeration of the lines from text
    /// </summary>
    public interface ITextParser
    {
        IEnumerable<string> Parse();
    }
}
