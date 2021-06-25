using System;
using System.Linq;

namespace Deploy
{
    class Program
    {
        public static bool excludeInterception = false;
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                var curdir = System.IO.Directory.GetCurrentDirectory();
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"..\..\..\..\ByulMacro\bin\Debug\\net5.0-windows");
                foreach (var file in di.GetFiles())
                {

                }
            }
            else
            {
                foreach(var arg in args)
                {
                    if (arg == "-no-interception")
                    {
                        excludeInterception = true;
                    } 
                }
            }
        }
    }
}
