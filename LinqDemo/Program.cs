using System;
using System.Collections.Generic;
using System.Linq;

using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace LinqDemo
{
    public class LinqDemo
    {
        public static void Main(string[] args)
        {
            string nl = Environment.NewLine;
            Console.WriteLine(
                "Here are some demos using Linq. "
                + "See also 101 Linq Samples for Visual C#, at:"
                + nl
                + "\thttp://msdn.microsoft.com/en-us/vstudio/aa336746"
                + nl);

            var classes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type =>
                    type.IsClass
                    && type.GetCustomAttributes(typeof(LinqDemoClassAttribute), false).Length > 0
                    );
            IEnumerable<MethodInfo> staticMethods = classes
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static));
            IEnumerable<MethodInfo> taggedMethods = staticMethods
                .Where(method => method.GetCustomAttributes(
                    typeof(LinqDemoMethodAttribute), false).Length > 0);
            var orderedMethods = taggedMethods.OrderBy(method => method.Name);

            //foreach (var action in new Action[] { /* Explicit list */ } ) { action(); Util.Pause(); }
            foreach (MethodInfo mi in orderedMethods)
            {
                string description = String.Empty;
                DescriptionAttribute descriptor = (DescriptionAttribute)
                    mi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                if (descriptor != null)
                {
                    Console.WriteLine("Demo description: {0:s}", descriptor.Description);
                }
                mi.Invoke(null, new Object[0]);
                Util.Pause();
            }
            Console.WriteLine("Press enter to exit.");
            Console.Out.Flush();
            Console.ReadLine();
        }
    }
}