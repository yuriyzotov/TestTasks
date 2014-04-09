using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextMetrics.Core.Aggregators;
using System.Linq;

namespace TextMetrics.Core.Test
{
    [TestClass]
    public class TokensFrequencyAggregatorTest
    {
        [TestMethod]
        public void TestAsyncAdding()
        {
            var input = new List<string>() { "АннА", "Attendez", "сказала", "и","анна", "я", "Анна", "Павловна", "соображая", "Я", "нынче", "же", "поговорю",
                "Lise", "la", "femme", "du", "jeune", "Болконский", "И" };
            var aggregator = new TokensFrequencyAggregator();
            var expected = new Dictionary<string, long>()
            {
                {"анна",3},
                {"и",2},
                {"я",2},
                {"attendez",1},
                {"сказала",1},
                {"павловна",1},
                {"соображая",1},
                {"нынче",1},
                {"же",1},
                {"lise",1},
                {"la",1},
                {"femme",1},
                {"jeune",1},
                {"du",1},
                {"болконский",1},
                {"поговорю",1}
            };
            input.AsParallel().All(i=>{aggregator.Aggregate(i); return true;});
            var output = aggregator.GetAggregation().ToList();
            Assert.AreEqual(expected["анна"], output[0].Value);
            CollectionAssert.AreEquivalent(expected.ToList(), output);
        }

        [TestMethod]
        public void TestEmptyAdding()
        {
            var aggregator = new TokensFrequencyAggregator();
            aggregator.Aggregate("");
            aggregator.Aggregate(null);
            aggregator.Aggregate(null);
            aggregator.Aggregate("");
            Assert.AreEqual(0, aggregator.GetAggregation().Count());
        }

    }
}
