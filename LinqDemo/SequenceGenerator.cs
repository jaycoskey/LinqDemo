using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    /// <remarks>Based on code written by Eric Lippert.</remarks>
    [LinqDemoClass]
    public class SequenceGenerator
    {
        [LinqDemoMethod]
        [Description("Iterate through increasing lists of an underlying set of items, using two different methods.")]
        public static void DemoSequenceGenerator()
        {
            Util.Entering();

            int seqLength = 3;
            int min = 0;
            int max = 2;

            List<List<int>> seq1 = new List<List<int>>();
            foreach (var sequence in SequenceGenerator1(seqLength, min, max))
            {
                seq1.Add(sequence.ToList());
            }

            List<List<int>> seq2 = new List<List<int>>();
            foreach (var sequence in SequenceGenerator2(seqLength, min, max))
            {
                seq2.Add(sequence.ToList());
            }

            var seqPairs = seq1.Zip(seq2, (fst, snd) => new { First = fst, Second = snd });
            foreach (var seqPair in seqPairs)
            {
                Console.WriteLine("{0:s}\t<==>\t{1:s}",
                    String.Join(", ", seqPair.First),
                    String.Join(", ", seqPair.Second));
            }
        }

        private static IEnumerable<IEnumerable<int>> SequenceGenerator1(int seqLength, int min, int max)
        {
            if (seqLength == 0)
            {
                yield return Enumerable.Empty<int>();
            }
            else
            {
                for (int first = min; first <= max; first++)
                {
                    foreach (var tail in SequenceGenerator1(seqLength - 1, first, max))
                    {
                        yield return Util.Prepend(first, tail);
                    }
                }
            }
        }

        /// <remarks>Based on code written by Eric Lippert.</remarks>
        private static IEnumerable<IEnumerable<int>> SequenceGenerator2(int seqLength, int min, int max)
        {
            if (seqLength == 0)
            {
                return Singleton(Enumerable.Empty<int>());
            }
            else
            {
                var query =
                    from first in Enumerable.Range(min, max - min + 1)
                    from tail in SequenceGenerator2(seqLength - 1, first, max)
                    select Util.Prepend(first, tail);
                return query;
            }
        }

        public static IEnumerable<T> Singleton<T>(T first)
        {
            yield return first;
        }
    }
}