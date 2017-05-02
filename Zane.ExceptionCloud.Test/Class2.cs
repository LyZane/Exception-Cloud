using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Zane.ExceptionCloud.Test
{
    public static class Class2
    {
        public static string c2()
        {
            int.Parse("abc");
            return "";
        }
    }
}
