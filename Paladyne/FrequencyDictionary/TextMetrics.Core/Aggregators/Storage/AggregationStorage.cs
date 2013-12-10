﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace TextMetrics.Core.Aggregators.Storage
{
    public class TextFileAggregationStorage : IAggregeationStorage
    {
        private string myOutputFile;
        private Encoding myEncoding;

        public TextFileAggregationStorage(string filename, Encoding encoding)
        {
            myOutputFile = filename;
            myEncoding = encoding;
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
