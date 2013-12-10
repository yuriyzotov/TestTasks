using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextMetrics.Core.Parsers;
using System.Linq;
using System.Collections.Generic;

namespace TextMetrics.Core.Test
{
    [TestClass]
    public class FileTextParserTest
    {
        private string[] myFilePathes = new string[4];
        private List<string> myText = new List<string>() {
            "Отец очень богат и скуп. Он живет в деревне. Знаете, этот известный князь Болконский,",
            "отставленный еще при покойном императоре и прозванный прусским королем. Он очень умный человек,",
            "но со странностями и тяжелый. La pauvre petite est malheureuse comme les pierres 24.",
            "У нее брат, вот что недавно женился на Lise Мейнен, адъютант Кутузова. Он будет нынче у меня"
        };
        
        [TestInitialize]
        public void Init()
        {
         
            //create temporary files
            myFilePathes[0] = Path.GetTempFileName();
            myFilePathes[1] = Path.GetTempFileName();
            myFilePathes[2] = Path.GetTempFileName();
            myFilePathes[3] = Path.GetTempFileName();
            using(var sw = new StreamWriter(myFilePathes[0],false,Encoding.UTF32))
            {
                foreach (var s in myText)
                {
                    sw.WriteLine(s);
                }
            }
            using (var sw = new StreamWriter(myFilePathes[1], false, Encoding.UTF8))
            {
                foreach (var s in myText)
                {
                    sw.WriteLine(s);
                }
            }
            using (var sw = new StreamWriter(myFilePathes[2], false, Encoding.GetEncoding(1251)))
            {
                foreach (var s in myText)
                {
                    sw.WriteLine(s);
                }
            }
            using (var sw = new FileStream(myFilePathes[3],FileMode.OpenOrCreate))
            {
                var rnd = new Random();
                var data = new byte [1024];
                rnd.NextBytes(data);
                sw.Write(data,0,data.Length);
            }

        }

        [TestMethod]
        public void TestFileParserEncoding()
        {
            //test load dufferent file types

            ITextParser parser;
            try
            {
                parser = new FileTextParser(myFilePathes[0], Encoding.GetEncoding(1251));
                var outputNon = parser.Parse();
                Assert.Fail("Invalid encoding detection");
            }
            catch (ApplicationException)
            { }

            parser = new FileTextParser(myFilePathes[1], Encoding.GetEncoding(1251));
            var output = parser.Parse();
            CollectionAssert.AreEquivalent(myText,output.ToList());

            parser = new FileTextParser(myFilePathes[2], Encoding.GetEncoding(1251));
            output = parser.Parse();
            CollectionAssert.AreEquivalent(myText, output.ToList());

            
            try
            {
                parser = new FileTextParser(myFilePathes[3], Encoding.GetEncoding(1251));
                var outputNon = parser.Parse();
                Assert.Fail("Try to open binary file");
            }
            catch (ApplicationException)
            { }

        }


        [TestMethod]
        public void TestEmptyFileParserEncoding()
        {
            var parser = new FileTextParser(Path.GetTempFileName(),Encoding.GetEncoding(1251));
            var output = parser.Parse();
            Assert.AreEqual(0, output.Count());
        }
        [TestMethod]
        public void TestNonExistingFile()
        {
            try
            {
                var parser = new FileTextParser(Guid.NewGuid().ToString(), Encoding.GetEncoding(1251));
                Assert.Fail("Try to use not exist file.");
            }
            catch (FileNotFoundException)
            {

            }
        }
    }

}
