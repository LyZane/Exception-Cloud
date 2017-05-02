using ExceptionCloud;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Zane.ExceptionCloud.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            GlobalConfiguration.Configuration.CatchGlobeException().Startup();
            var result = new Class1().c1();
            Console.WriteLine(result);
            Console.ReadKey();
        }

        
    }
}
