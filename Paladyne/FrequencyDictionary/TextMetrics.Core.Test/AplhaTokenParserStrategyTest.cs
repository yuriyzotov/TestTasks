using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TextMetrics.Core.Parsers;
using System.Text;

namespace TextMetrics.Core.Test
{
    [TestClass]
    public class AplhaTokenParserStrategyTest
    {
        [TestMethod]
        public void TestRussianSentenceWithNumericValues()
        {
            var inputText = "1. - Во-первых объём, О'тец очень богат и скуп. Он живет в деревне. Знаете, этот известный князь Болконский, отставленный еще при покойном императоре и прозванный прусским королем. Он очень умный человек, но со странностями и тяжелый. La pauvre petite est malheureuse comme les pierres24.";
            var expected = new List<string>()
            {
                "Во-первых", "объём",
                "О'тец",
                "очень",
                "богат", "и", "скуп", "Он", "живет", "в", "деревне","Знаете","этот",
                "известный", "князь", "Болконский", "отставленный", "еще", "при" ,
                "покойном", "императоре", "и", "прозванный", "прусским", "королем",
                "Он", "очень", "умный", "человек", "но", "со","странностями","и", "тяжелый","La","pauvre","petite","est","malheureuse","comme","les", "pierres"
                
            };
            var strategy = new AlphaTokensParserStrategy();
            var output = strategy.Parse(inputText);
            CollectionAssert.AreEquivalent(expected,output.ToList());
        }


        [TestMethod]
        public void TestStringWithoutAlpha()
        {
            var inputText = "- ' 123 -3-3   ";
            var expected = new List<string>();
            var strategy = new AlphaTokensParserStrategy();
            var output = strategy.Parse(inputText);
            CollectionAssert.AreEquivalent(expected, output.ToList());
        }
        [TestMethod]
        public void TestEmptyString()
        {
            var inputText = "";
            var expected = new List<string>();
            var strategy = new AlphaTokensParserStrategy();
            var output = strategy.Parse(inputText);
            CollectionAssert.AreEquivalent(expected, output.ToList());
        }

        [TestMethod]
        public void TestNullString()
        {
            string inputText = null;
            var expected = new List<string>();
            var strategy = new AlphaTokensParserStrategy();
            var output = strategy.Parse(inputText);
            CollectionAssert.AreEquivalent(expected, output.ToList());
        }

    }
}
