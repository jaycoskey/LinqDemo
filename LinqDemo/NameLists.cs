using System;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    /// <summary>Class to demonstrate Linq set and grouping methods</summary>
    [LinqDemoClass]
    class NameLists
    {
        [LinqDemoMethod]
        [Description("Demonstration of set processing with Linq")]
        public static void DemoNameLists()
        {
            Util.Entering();

            string[] listA = { "Chris", "Christina", "Amy", "Amy", "Amy", "Andy", "Arnold" };
            string[] listB = { "Chris", "Christina", "Ben", "Ben", "Beth", "Bob" };
            Console.WriteLine("\tList A: {0:s}", String.Join(", ", listA));
            Console.WriteLine("\tList B: {0:s}", String.Join(", ", listB));
            var inCommon = listA.Intersect(listB);
            Console.WriteLine("\tIn common: {0:s}", String.Join(", ", inCommon));
            var onlyInA = listA.Except(listB);
            Console.WriteLine("\tIn listA only: {0:s}", String.Join(", ", onlyInA));
            var onlyInB = listB.Except(listA);
            Console.WriteLine("\tIn listB only: {0:s}", String.Join(", ", onlyInB));
            var symmetricDifference = onlyInA.Union(onlyInB);
            Console.WriteLine("\tSymmetric difference: {0:s}", String.Join(", ", symmetricDifference));

            var histogramByFirstLetter = listA.Union(listB).GroupBy(name => name[0])
                .OrderBy(grp => grp.Key)
                .Select(grp => new { grp.Key, grp });
            Console.WriteLine("    Names grouped by first letter:");
            foreach (var histo in histogramByFirstLetter)
            {
                Console.WriteLine("\t{0:s}: {1:s}",
                    histo.Key.ToString(),
                    String.Join(", ", histo.grp));
            }
        }
    }
}