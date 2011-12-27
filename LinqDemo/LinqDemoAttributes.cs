using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqDemo
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    class LinqDemoClassAttribute : System.Attribute { }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    class LinqDemoMethodAttribute : System.Attribute { }
}