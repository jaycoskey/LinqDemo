using System;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;
using System.Reflection;

namespace LinqDemo
{
    public static class Util
    {
        public static void Entering()
        {
            Console.WriteLine("Entering {0:s}....", Util.GetCurrentMethodName(2));
        }
        public static string GetCurrentMethodName(int stackDepth = 1) {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(stackDepth);
            MethodBase mb = sf.GetMethod();
            return mb.Name;
        }

        /// <remarks>Based on code written by Eric Lippert.</remarks>
        public static IEnumerable<T> Prepend<T>(T first, IEnumerable<T> tail)
        {
            yield return first;
            foreach (var item in tail) { yield return item; }
        }

        public static void Pause()
        {
            PressEnter("Press enter to continue: ");
        }

        /// <summary>Provide Python-like array slices</summary>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int nBegin, int nEnd)
        {
            var subset = source.Skip(nBegin).Take(nEnd - nBegin + 1);
            return subset;
        }

        #region Private methods
        private static void PressEnter(string prompt)
        {
            Console.WriteLine();
            Console.Write(prompt);
            Console.ReadLine();
            Console.WriteLine();
        }
        #endregion // Private methods
    }
}