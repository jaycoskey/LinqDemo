using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    /// <summary>
    ///     This class demonstrates code involving permutation iteration.
    ///     It makes use of IEnumerable<>, but not much of Linq itself.
    /// </summary>
    [LinqDemoClass]
    class PermutationIterator
    {
        #region Types
        private struct IndexMapping
        {
            public IndexMapping(int index, int newIndex)
            {
                this.Index = index;
                this.NewIndex = newIndex;
            }
            public int Index;
            public int NewIndex;
        }
        #endregion // Types

        #region Public methods
        [LinqDemoMethod]
        [Description("Iterate through all permutations of a given (small) set.")]
        public static void DemoPermutationIterator()
        {
            Util.Entering();

            int[] a = { 1, 9, 4 };

            Console.WriteLine("Permutations of ({0:s}): ", String.Join(", ", a));
            foreach (var permutation in GetAllPermutations(a))
            {
                var permutationStr = permutation.Select(i => i.ToString());
                Console.WriteLine(String.Join(", ", permutationStr));
            }
        }

        public static IEnumerable<IEnumerable<int>> GetAllPermutations(int[] items)
        {
            return Util.Prepend(items.Select(i => i), GetPermutations(items));
        }

        /// <see>Dijkstra, Edsger W.,  A Discipline of Programming. Prentice-Hall, 1997.</see>
        /// <summary>
        ///     * Find the largest i for which a[i] < a[i+i]
        ///     * Replace a[i] with the next largest value in ( a[i+1] ... a[n] }.
        ///     * Reverse (a[i+1] ... [a[n]).  (I.e., set to lex. increasing order).
        /// </summary>
        public static IEnumerable<IEnumerable<int>> GetPermutations(int[] items)
        {
            int length = items.Length;
            IndexMapping[] indexPermutation = new IndexMapping[length];
            for (int i = 0; i < length; i++) { indexPermutation[i] = new IndexMapping(i, i); }
            while (true)
            {
                int i = length - 2;
                // Find largest non-decreasing final subsequence.
                for (; i >= 0 && indexPermutation[i].Index >= indexPermutation[i + 1].Index; i--) { }
                if (i < 0) { yield break; }  // Seq indices are in decreasing order
                int j = length - 1;
                // Find number to the right of a[i] that is next-largest.
                for (; j > i && indexPermutation[i].Index >= indexPermutation[j].Index; j--) { }
                Swap(ref indexPermutation[i], ref indexPermutation[j]);
                Array.Reverse(indexPermutation, i + 1, length - i - 1);
                yield return applyIndexPermutation(items, indexPermutation);
            }
        }

        public static void Swap<T>(ref T x, ref T y)
        {
            T tmp = x;
            x = y;
            y = tmp;
        }
        #endregion // Public methods

        #region Private methods
        private static IEnumerable<T> applyIndexPermutation<T>(T[] items, IndexMapping[] indexPermutation)
        {
            for (int i = 0; i < indexPermutation.Length; i++)
            {
                yield return items[indexPermutation[i].NewIndex];
            }
        }
        
        private static void printIndexValuePairArray(IndexMapping[] indexValues)
        {
            foreach (var indexValue in indexValues)
            {
                Console.WriteLine("  {0:s}",
                    String.Join(" - ", indexValues.Select(
                        iv => String.Format("({0:d},{1:d})", iv.NewIndex, iv.Index))));
            }
        }
        #endregion // Private methods
    }
}