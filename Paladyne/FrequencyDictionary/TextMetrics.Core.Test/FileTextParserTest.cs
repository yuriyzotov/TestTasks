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
        
        private List<string> myText = new List<string>() {
            "Отец очень богат и скуп. Он живет в деревне. Знаете, этот известный князь Болконский,",
            "отставленный еще при покойном императоре и прозванный прусским королем. Он очень умный человек,",
            "но со странностями и тяжелый. La pauvre petite est malheureuse comme les pierres 24.",
            "У нее брат, вот что недавно женился на Lise Мейнен, адъютант Кутузова. Он будет нынче у меня"
        };
        
        [TestInitialize]
        public void Init()
        {
         
          
          

        }

        //used to prepare temp file
        private string CreateTestFileWithEncoding(Encoding encoding)
        {
            var filePath = Path.GetTempFileName();
            //prepare file
            using (var sw = new StreamWriter(filePath, false, encoding))
            {
                foreach (var s in myText)
                {
                    sw.WriteLine(s);
                }
            }
            return filePath;
        }

        [TestMethod]
        public void TestFileParserWithInvalidEncoding()
        {
            ITextParser parser;
            //create temporary files
            var filePath = CreateTestFileWithEncoding(Encoding.UTF32);
            //load file
            try
            {
                parser = new FileTextParser(filePath, Encoding.GetEncoding(1251));
                var outputNon = parser.Parse();
                Assert.Fail("Invalid encoding detection");
            }
            catch (ApplicationException)
            {
                //all ok we are supposed to have this exception
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestFileParserWithValidUTF8Encoding()
        {
            ITextParser parser;
            //create temporary files
            var filePath = CreateTestFileWithEncoding(Encoding.UTF8);
            //load file
            try
            {
                parser = new FileTextParser(filePath, Encoding.GetEncoding(1251));
                var output = parser.Parse();
                CollectionAssert.AreEquivalent(myText, output.ToList());
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestFileParserWithValidCyrillicEncoding()
        {
            ITextParser parser;
            //create temporary files
            var filePath = CreateTestFileWithEncoding(Encoding.GetEncoding(1251));
            //load file
            try
            {
                parser = new FileTextParser(filePath, Encoding.GetEncoding(1251));
                var output = parser.Parse();
                CollectionAssert.AreEquivalent(myText, output.ToList());
            }
            finally
            {
                File.Delete(filePath);
            }
        }

     
        [TestMethod]
        public void TestFileParserWithBinaryFileEncoding()
        {
            ITextParser parser;
            //create temporary files
            var filePath = Path.GetTempFileName();
            //prepare file
            using (var sw = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                var rnd = new Random();
                var data = new byte[1024];
                rnd.NextBytes(data);
                sw.Write(data, 0, data.Length);
            }
            //load file
            try
            {
                parser = new FileTextParser(filePath, Encoding.GetEncoding(1251));
                var outputNon = parser.Parse();
                Assert.Fail("Try to open binary file");
            }
            catch (ApplicationException)
            {
                //all ok we are supposed to have this exception
            }
            finally
            {
                File.Delete(filePath);
            }
        }
        
        [TestMethod]
        public void TestEmptyFileParser()
        {
            var parser = new FileTextParser(Path.GetTempFileName(),Encoding.GetEncoding(1251));
            var output = parser.Parse();
            Assert.AreEqual(0, output.Count());
        }
        [TestMethod]
        public void TestFileParserWithNullEncoding()
        {
            ITextParser parser;
            //create temporary files
            var filePath = CreateTestFileWithEncoding(Encoding.GetEncoding(1251));
            //load file
            try
            {
                parser = new FileTextParser(filePath, null);
                var output = parser.Parse();
                CollectionAssert.AreEquivalent(myText, output.ToList());
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestNonExistingFile()
        {
            try
            {
                var parser = new FileTextParser(Guid.NewGuid().ToString(), Encoding.GetEncoding(1251));
                Assert.Fail("Try to use file which not exist.");
            }
            catch (FileNotFoundException)
            {
                //all ok we are supposed to have this exception
            }
        }
    }

}
