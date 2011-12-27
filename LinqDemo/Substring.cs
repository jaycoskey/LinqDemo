using System;
using System.Linq;

using System.ComponentModel;
using System.Globalization;

namespace LinqDemo
{
    [LinqDemoClass]
    class Substring
    {
        [LinqDemoMethod]
        [Description("Demo of substrings searching with Linq.  Compare with Regex class.")]
        public static void DemoSubstringCount()
        {
            Util.Entering();

            string haystack = "The nature of their theme.";
            string needle = "the";
            
            // CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            Console.WriteLine("Haystack=\"{0:s}\"" + Environment.NewLine
                + "Needle=\"{1:s}\"" + Environment.NewLine
                + "\tOccurrences of needle in haystack, case-sensitive: {2:d}" + Environment.NewLine
                + "\tOccurrences of needle in haystack, case-insensitive: {3:d}",
                haystack,
                needle,
                SubstringInstanceCount(haystack, needle, false, cultureInfo),
                SubstringInstanceCount(haystack, needle, true, cultureInfo));
        }
        public static int SubstringInstanceCount(
            string haystack,
            string needle,
            bool ignoreCase,
            CultureInfo cultureInfo)
        {
            Func<string, bool> pred =
                substring => substring.StartsWith(needle, ignoreCase, cultureInfo);
            var query = haystack.Select(
                (c, i) => haystack.Substring(i))
                    .Count(substring => pred(substring));
            return query;
        }
    }
}