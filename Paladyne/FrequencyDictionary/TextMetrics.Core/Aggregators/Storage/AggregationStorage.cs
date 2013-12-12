using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace TextMetrics.Core.Aggregators.Storage
{
    /// <summary>
    /// save aggreagation in the text file with special format
    /// </summary>
    public class TextFileAggregationStorage : IAggregeationStorage
    {
        private string myOutputFile;
        private Encoding myEncoding;

        public TextFileAggregationStorage(string filename, Encoding encoding)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("Output file");

            myOutputFile = filename;
            this.myEncoding = encoding ?? Encoding.GetEncoding(1251);
        }

        public void SaveAggregation(ITokensAggregator aggregator)
        {
            using (var writer = new StreamWriter(myOutputFile, false, myEncoding))
            {
                //create output file
                foreach (var pair in aggregator.GetAggregation())
                {
                    writer.WriteLine("{0},{1}", pair.Key, pair.Value.ToString());
                }
            }
        }
    }
}
