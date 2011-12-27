using System;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    [LinqDemoClass]
    class CharacterFrequency
    {
        [LinqDemoMethod]
        [Description("A string's character distribution found using a Linq one-liner")]
        public static void DemoCharacterFrequency()
        {
            Util.Entering();

            string inputString = "abcdabcaba";
            Console.WriteLine("\tString = \"{0:s}\"", inputString);
            int maxFreq = GetMostFrequentCharCount(inputString);
            Console.WriteLine("\tHighest char frequency = {0:d}", maxFreq);
        }

        /// <summary>Returns the count of the most frequently appearing character.</summary>
        /// <param name="input">Input string guaranteed to be lowercase alpha (a-z)</param>
        /// <returns>The count of the most frequently appearing character</returns>
        /// <example> "abcdabcaba" => 4, since 'a' appears 4 times</example>
        /// <remark>
        ///     Note: Calling Count() on an IEnumerable is generally frowned upon,
        ///     but appropriate in this case.
        /// </remark>
        public static int GetMostFrequentCharCount(string input)
        {
            return input.GroupBy(c => c).OrderBy(grp => grp.Count()).Last().Count();
        }
    }
}