using ByulMacro.Core.Attributes;
using System; 
using System.Reflection; 
using System.Threading.Tasks;

namespace ByulMacro.Core
{
    class RunAttributeInitializer
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
                            var runAtt = method.GetCustomAttribute<Run>(); 
                            var runInTaskAtt = method.GetCustomAttribute<RunInTask>(); 
                            if(runInTaskAtt != null)
                            {
                                Task.Run(() => {
                                    try
                                    {
                                        if (runInTaskAtt.o == null)
                                        {
                                            method?.Invoke(null, null); // static call
                                        }
                                        else
                                        {
                                            method?.Invoke(runAtt.o, null);
                                        }
                                    }
                                    catch(Exception e)
                                    {
                                        if (e.GetType() == typeof(TargetException))
                                        {
                                            Console.WriteLine("Run In Task는 static 메서드에만 허용됩니다." + e.StackTrace);
                                        }
                                        else
                                        {
                                            Console.WriteLine(e);
                                        }
                                    }
                                 
                                }); 
                            }
                            if (runAtt != null)
                            {
                                try
                                {
                                    if(runAtt.o  == null)
                                    {
                                        method?.Invoke(null, null); // static call
                                    }
                                    else
                                    {
                                        method?.Invoke(runAtt.o, null);
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
