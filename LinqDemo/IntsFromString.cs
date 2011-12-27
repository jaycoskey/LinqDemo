using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    [LinqDemoClass]
    class IntsFromString
    {
        [LinqDemoMethod]
        [Description("Convert a csv-string of ints to an array of ints")]
        public static void DemoIntsFromString()
        {
            Util.Entering();

            var ints1 = Enumerable.Range(-3, 7).Concat(Enumerable.Repeat(5, 3));
            string intString = String.Join(",", ints1);
            Console.WriteLine("\tOriginal list of ints:  {0:s}", String.Join(",", intString));
            int[] ints2 = IntsFromString.GetIntsFromString(intString).ToArray();
            Console.WriteLine("\tRecovered list of ints: {0:s}", String.Join(",", ints2));
        }

        /// <summary>Demo of Linq one-liner</summary>
        public static IEnumerable<int> GetIntsFromString(string input, char delimiter = ',')
        {
            // Note: Split can take an array of chars, but not a ParallelQuery<>.
            // return input.AsParallel().Split(delimiter).Select(iStr => int.Parse(iStr));

            return input.Split(delimiter).AsParallel().Select(iStr => int.Parse(iStr));
        }
    }
}