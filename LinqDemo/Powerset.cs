using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    [LinqDemoClass]
    public class Powerset
    {
        public enum Color { Red, Green, Blue };

        [LinqDemoMethod]
        [Description("Display all subsets of a given (small) set")]
        public static void DemoPowerset()
        {
            Util.Entering();

            List<Color> colors = new List<Color> { Color.Red, Color.Green, Color.Blue };
            Console.WriteLine("\tSet: {0:s}", String.Join(", ", colors.Select(c => c.ToString())));

            printPowerset(Powerset.GetPowerset1(colors), "  Subsets (version 1):");
            printPowerset(Powerset.GetPowerset2(colors), "  Subsets (version 2):");
            printPowerset(Powerset.GetPowerset3(colors), "  Subsets (version 3):");
        }

        private static void printPowerset(IEnumerable<IEnumerable<Color>> powerset, string label)
        {
            Console.WriteLine("=====" + label + "=====");
            foreach (var subset in powerset)
            {
                string str = String.Join(", ", subset);
                Console.WriteLine("\t{0:s}", str == string.Empty ? "<<Empty set>>" : str);
            }
        }

        /// <remark>
        ///     Treach each subset # from 0 to 2^n - 1 as a binary vector.
        ///     The binary digit at position k tells whether list[k] is in that subset.
        /// </remark>
        /// <remarks>
        ///     Note: This is more verbose than how it would be written in Haskell:
        ///     > powerset = foldr (\x acc -> acc ++ map (x:) acc) [[]]
        ///     > powerset = map concat . mapM (\x -> [[],[x]])
        ///     > powerset = filterM (const [True, False])
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> GetPowerset1<T>(List<T> list)
        {
            var seed = new List<IEnumerable<T>>() { Enumerable.Empty<T>() }
                as IEnumerable<IEnumerable<T>>;
            // Fold concatenation approach
            var query1 = list.Aggregate(
                seed,
                (subsets, item) => subsets.Concat(
                    subsets.Select(s => s.Concat(new List<T>() { item }))
                    )
                );
            return query1;
        }

        public static IEnumerable<IEnumerable<T>> GetPowerset2<T>(List<T> list)
        {
            // Numbered subset approach, using fluent syntax:
            var query2 = Enumerable.Range(0, 1 << list.Count)
                .Select(subsetNum => Enumerable.Range(0, list.Count)
                    .Where(itemNum => (subsetNum & (1 << itemNum)) != 0)
                    .Select(itemNum => list[itemNum]));
            return query2;
        }

        public static IEnumerable<IEnumerable<T>> GetPowerset3<T>(List<T> list) {
            // Numbered subset approach, using query syntax:
            var query3 = from subsetNum in Enumerable.Range(0, 1 << list.Count)
                        select
                            from itemNum in Enumerable.Range(0, list.Count)
                            where ((subsetNum & (1 << itemNum)) != 0)
                            select list[itemNum];
            return query3;
        }
    }
}