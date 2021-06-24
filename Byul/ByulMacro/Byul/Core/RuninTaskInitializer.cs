using ByulMacro.Byul.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Core
{
    class RuninTaskInitializer
    {
        public static void Init()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            { 
                if (asm.FullName.Split(' ')[0].Contains("ByulMacro"))
                {
                    foreach(var type in asm.GetTypes())
                    {
                        foreach(var method in type.GetMethods())
                        {
                            var att = method.GetCustomAttribute<Run>(); 
                            if(att != null)
                            {
                                try
                                {
                                    if(att.o  == null)
                                    {
                                        method?.Invoke(null, null); // static call
                                    }
                                    else
                                    {
                                        method?.Invoke(att.o, null);
                                    }
                                   
                                }
                                catch(Exception e)
                                {
                                    if(e.GetType() == typeof(TargetException))
                                    {
                                        Console.WriteLine("Run In Task는 static 메서드에만 허용됩니다." + e.StackTrace);
                                    }
                                    else
                                    {
                                        Console.WriteLine(e);
                                    }
                                }
                           
                            }
                        }

                    }
                }
            }

        }
    }
}
