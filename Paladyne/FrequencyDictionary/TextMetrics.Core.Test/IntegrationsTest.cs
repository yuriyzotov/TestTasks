using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TextMetrics.Core.TextMetrics;
using TextMetrics.Core.Parsers;
using TextMetrics.Core.Aggregators;
using TextMetrics.Core.Aggregators.Storage;

namespace TextMetrics.Core.Test
{
    [TestClass]
    public class IntegrationsTest
    {

        [TestMethod]
        public void TestHugeFile()
        {

            var words = new Dictionary<string, long>();
            while (words.Keys.Count < 10)
            {
                var word = GenerateWord();
                words[word] = 0;
            }

            //generate file and calculate metrics
            var filePath = Path.GetTempFileName();
            GenerateHugeFile(words, filePath);

            var outFileName = filePath + ".out";
            var parser = new TextMetricsCalculator(new ConfigurationMock(filePath, outFileName));
            parser.Calculate(true);

            try
            {
                CheckResults(words, outFileName);
            }
            finally
            {
                File.Delete(filePath);
                File.Delete(outFileName);
            }
        }





        private class ConfigurationMock : ITextMetricsConfiguration
        {
            private string myInPath;
            private string myOutPath;
            private Encoding myEncoding = Encoding.GetEncoding(1251);

            public ConfigurationMock(string inPath, string outPath)
            {
                this.myInPath = inPath;
                this.myOutPath = outPath;
            }

            public Parsers.ITextParser CreateTextParser()
            {
                return new FileTextParser(myInPath, myEncoding);
            }

            public Parsers.ITokensParserStrategy CreateTokenParserStrategy()
            {
                return new AlphaTokensParserStrategy();
            }

            public Aggregators.ITokensAggregator CreateTokenAggregator()
            {
                return new TokensFrequencyAggregator();
            }

            public Aggregators.Storage.IAggregeationStorage CreateAggregationStorage()
            {
                return new TextFileAggregationStorage(myOutPath, myEncoding);
            }
        }


        private string myLetters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя-'";


        private string GenerateWord()
        {
            var rnd = new Random();
            var wordLength = rnd.Next(1, 11);
            var word = "";
            for (int i = 0; i < wordLength; i++)
            {
                var index = rnd.Next(0, myLetters.Length);
                word += myLetters[index];
            }
            return word;
        }

        private static void CheckResults(Dictionary<string, long> words, string outFileName)
        {
            //check results
            using (var fr = new StreamReader(outFileName, Encoding.GetEncoding(1251)))
            {
                var line = "";
                var value = 0;
                var prevValue = 0;

                while ((line = fr.ReadLine()) != null)
                {
                    var values = line.Split(",".ToCharArray());
                    var key = words.Keys.SingleOrDefault(k => k.ToLowerInvariant() == values[0]);
                    if (key == null)
                    {
                        Assert.Fail("Key '{0}' not found", values[0]);
                    }
                    value = int.Parse(values[1]);
                    Assert.AreEqual(words[key], value);
                    //check sorting
                    if (prevValue != 0)
                    {
                        Assert.IsTrue(value <= prevValue);
                    }
                    prevValue = value;

                }
            }
        }

        private static void GenerateHugeFile(Dictionary<string, long> words, string filePath)
        {
            var rnd = new Random();
            using (var sw = new StreamWriter(filePath, false, Encoding.GetEncoding(1251)))
            {
                var buffer = new StringBuilder();
                for (int i = 0; i < 100 * 1024 * 1024; i++)
                {
                    var index = rnd.Next(0, 13);
                    switch (index)
                    {
                        case 12:
                            buffer.Append(",");
                            break;
                        case 11:
                            buffer.Append(".");
                            break;
                        case 10:
                            sw.WriteLine(buffer.ToString());
                            buffer.Clear();
                            break;
                        default:
                            var word = words.Keys.ElementAt(index);
                            buffer.Append(word);
                            buffer.Append(" ");
                            words[word] += 1;
                            break;
                    }
                }
                if (buffer.Length != 0)
                    sw.WriteLine(buffer.ToString());

            }
        }
    }
}
