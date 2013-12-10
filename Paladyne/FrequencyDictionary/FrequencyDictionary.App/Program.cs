using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TextMetrics.Core.TextMetrics;


namespace FrequencyDictionary.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //parse input arguments
                var parser = new TextMetricsCalculator(new Configuration(args));
                //make calculation of metric
                parser.Calculate(true);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error is: {0}",ex.Message);
            }
        }
    }
}
