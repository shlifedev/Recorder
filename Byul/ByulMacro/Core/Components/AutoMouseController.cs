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
        public static int Delay = 10;
        public static bool IsRun = false;

        [RunInTask]
        public static void AddHook()
        {        
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("add Hook");
            Hook.AddKeyboardEvent(VirtualKeyCode.Y, KeyState.Down, () => { 
                IsRun = true;
            });
            Hook.AddKeyboardEvent(VirtualKeyCode.Y, KeyState.Up, () => {
                IsRun = false;
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
                    Hook.IO.MouseUp(LowLevelInput.Hooks.VirtualKeyCode.Lbutton);

                }
                else
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }   
    }
}
