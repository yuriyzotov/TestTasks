using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TextMetrics.Core.Aggregators;
using TextMetrics.Core.Aggregators.Storage;
using TextMetrics.Core.Parsers;
using TextMetrics.Core.TextMetrics;

namespace FrequencyDictionary.App
{
    /// <summary>
    /// define configuration for text metrics in the console application
    /// </summary>
    public class Configuration : ITextMetricsConfiguration
    {
        
        private Encoding myEncoding = Encoding.GetEncoding(1251);
        private string myInputFileName;
        private string myOutputFileName;
        

        public Configuration(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("Input arguments");

            if (args.Length < 2)
                throw new ArgumentOutOfRangeException("Should be setup two arguments: input file name and output file name");



            myInputFileName = args[0];
            myOutputFileName = args[1];
            
            //setup encoding by value from config
            try
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["FileEncoding"]))
                    myEncoding = Encoding.GetEncoding(ConfigurationManager.AppSettings["FileEncoding"]);
            }
            catch (ArgumentException)
            {
                myEncoding = Encoding.GetEncoding(1251);
                Console.Out.WriteLine("Cannot use encoding '{0}' from config, use default '{1}' instead", ConfigurationManager.AppSettings["FileEncoding"],
                    myEncoding.WebName);
            }
        }

        public ITextParser CreateTextParser()
        {
            return new FileTextParser(myInputFileName, myEncoding);
        }

        public ITokensParserStrategy CreateTokenParserStrategy()
        {
            return new AlphaTokensParserStrategy();
        }

        public ITokensAggregator CreateTokenAggregator()
        {
            return new TokensFrequencyAggregator();
        }

        public IAggregeationStorage CreateAggregationStorage()
        {
            return new TextFileAggregationStorage(myOutputFileName, myEncoding);
        }
    }
}
