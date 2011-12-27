using System;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    [LinqDemoClass]
    class ArraySlice
    {
        [LinqDemoMethod]
        [Description("Demonstration of Python-like array slices")]
        public static void DemoArraySlice() {
            Util.Entering();

            var ints = Enumerable.Range(0, 10).ToArray();
            Console.WriteLine("\tOriginal array: {0:s}", String.Join(", ", ints));
            int nBegin = 3;
            int nEnd = 6;
            var someInts = ints.Slice(nBegin, nEnd).ToArray();
            Console.WriteLine("\tSliced array[{0:d}, {1:d}]: {2:s}",
                nBegin, nEnd, String.Join(", ", someInts));
        }
    }
}