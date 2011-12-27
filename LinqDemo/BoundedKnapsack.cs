using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    /// <summary>Utility class used to sort and de-dupe knapsack solutions</summary>
    public enum KnapsackItem { Small, Medium, Large };

    public class ListOfKnapsackItems
            : IComparer<List<KnapsackItem>>,
            IEqualityComparer<List<KnapsackItem>>
    {
        public int Compare(List<KnapsackItem> x, List<KnapsackItem> y)
        {
            using (var xIter = x.GetEnumerator())
            using (var yIter = y.GetEnumerator())
            {
                while (true)
                {
                    bool xNext = xIter.MoveNext();
                    bool yNext = yIter.MoveNext();
                    if (!(xNext || yNext))
                    {
                        return 0;
                    }
                    if (!xNext) { return -1; }
                    if (!yNext) { return 1; }
                    int comp = Math.Sign(yIter.Current - xIter.Current);
                    if (comp != 0) { return comp; }
                }
            }
        }

        public bool Equals(List<KnapsackItem> list1, List<KnapsackItem> list2)
        {
            return (new ListOfKnapsackItems()).Compare(list1, list2) == 0;
        }

        public int GetHashCode(List<KnapsackItem> list)
        {
            int hashCode = 0;
            int[] somePrimes = new int[] { 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
            int itemNum = 0;
            foreach (int i in list)
            {
                hashCode *= 2;
                hashCode += i * somePrimes[itemNum++ % somePrimes.Length];
            }
            return hashCode;
        }
    }

    [LinqDemoClass]
    public class BoundedKnapsack
    {
        [LinqDemoMethod]
        [Description("A brute force solution to the bounded knapsack problem with exact weight constraints.")]
        public static void DemoBoundedKnapsack() {
            Util.Entering();

            Dictionary<KnapsackItem, int> bounds = new Dictionary<KnapsackItem, int>
                { {KnapsackItem.Small,4}, {KnapsackItem.Medium,3}, {KnapsackItem.Large,1} };
            Dictionary<KnapsackItem, int> weights = new Dictionary<KnapsackItem, int>
                { {KnapsackItem.Small,10}, {KnapsackItem.Medium,20}, {KnapsackItem.Large,30} };
            Dictionary<KnapsackItem, int> worths = new Dictionary<KnapsackItem, int>
                { {KnapsackItem.Small,30}, {KnapsackItem.Medium, 20}, {KnapsackItem.Large,10} };
            int goal = 100;
            Console.WriteLine("\tItems: {0:s}",
                String.Join(", ", bounds.Keys.Select(e => e.ToString())));
            Console.WriteLine("\tBounds: {0:s}",
                String.Join(", ", bounds.Select(b => b.Key.ToString() + " => " + b.Value.ToString())));
            Console.WriteLine("\tWeights: {0:s}",
                String.Join(", ", weights.Select(b => b.Key.ToString() + " => " + b.Value.ToString())));
            Console.WriteLine("\tWorths: {0:s}",
                String.Join(", ", worths.Select(b => b.Key.ToString() + " => " + b.Value.ToString())));
            HashSet<List<KnapsackItem>> solutions = BoundedKnapsack.GetAllBoundedKnapsackSolutions_BruteForce(bounds, weights, worths, goal);
            Console.WriteLine("    Solutions:");
            if (solutions.Count == 0)
            {
                Console.WriteLine("\tNone");
            }
            else
            {
                int maxWorth = solutions.Max(solution => solution.Select(x => worths[x]).Sum());
                IEnumerable<List<KnapsackItem>> maxSolutions = solutions.Where(
                    solution => solution.Select(x => worths[x]).Sum() == maxWorth).Distinct(new ListOfKnapsackItems());
                foreach (List<KnapsackItem> solution in maxSolutions)
                {
                    Console.WriteLine("\tWorth={0:d} : {1:s}",
                        solution.Select(x => worths[x]).Sum(),
                        String.Join(", ", solution));
                }
            }
        }

        /// <summary>
        ///     Finds via exhaustive search all best solutions to the bounded knapsack problem with exact weight constraints.
        /// </summary>
        /// <param name="bounds">Dictionary: Keys=knapsack items, Values=max count of that item</param>
        /// <param name="weights">Dictionary: Keys=knapsack items, Values=item weights</param>
        /// <param name="worths">Dictionary: Keys=knapsack items, Values=worths (often called values)</param>
        /// <returns>The set of all solutions</returns>
        /// <remark>Potential optimization: Terminate if no solutions are possible
        ///     because the sum of all weights, including multiplicity, does not reach the goal.</remark> 
        public static HashSet<List<KnapsackItem>> GetAllBoundedKnapsackSolutions_BruteForce(
            Dictionary<KnapsackItem, int> bounds,
            Dictionary<KnapsackItem, int> weights,
            Dictionary<KnapsackItem, int> worths,
            int goal)
        {
            var result = new HashSet<List<KnapsackItem>>();
            foreach (KnapsackItem item in bounds.Keys)
            {
                if (weights[item] > goal) { continue; }
                if (weights[item] == goal)
                {
                    result.Add(new List<KnapsackItem> { item });
                }
                var itemHistogram2 = new Dictionary<KnapsackItem, int>(bounds);
                if (itemHistogram2[item]-- == 0)
                {
                    itemHistogram2.Remove(item);
                }
                HashSet<List<KnapsackItem>> subsolutions = BoundedKnapsack.GetAllBoundedKnapsackSolutions_BruteForce(
                    itemHistogram2, weights, worths, goal - weights[item]);
                foreach (List<KnapsackItem> subsolution in subsolutions)
                {
                    subsolution.Add(item);
                    subsolution.Sort();
                    result.Add(subsolution);
                }
            }
            return result;
        }
    }
}