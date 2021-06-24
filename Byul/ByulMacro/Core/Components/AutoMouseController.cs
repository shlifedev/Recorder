using ByulMacro.Core.Attributes;
using ByulMacro.Input;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Core.Components
{
    public static class AutoMouseController
    {
        public static int Delay = 2;
        public static bool IsRun = false;

        [Run]
        public static void AddHook()
        {
            Console.WriteLine("add Hook");
            Hook.AddKeyboardCombo(VirtualKeyCode.Lcontrol, VirtualKeyCode.Three, () => {
                Console.WriteLine("Hello");
                IsRun = !IsRun;
            }); 
        }

        [RunInTask]
        public static void RunInTaskAutoMouse()
        {
            while (true)
            {
                if (IsRun)
                {
                    System.Threading.Thread.Sleep(Delay);
                    Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Lbutton);

                }
                else
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }   
    }
}
