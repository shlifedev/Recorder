using AutoHamster.Core.Attributes;
using AutoHamster.Input;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHamster.Core.Components
{
    public static class AutoMouseController
    {
        public static int Delay = 10;
        public static bool IsRun = false;

        [RunInTask]
        public static void AddHook()
        {
            System.Threading.Thread.Sleep(1000); 
            Hook.AddMouseEvent(VirtualKeyCode.Xbutton2, KeyState.Down, delegate { IsRun = true; }); 
            Hook.AddMouseEvent(VirtualKeyCode.Xbutton2, KeyState.Up, delegate { IsRun = false; }); 
        }

        [RunInTask]
        public static void RunInTaskAutoMouse()
        {
            while (true)
            {
                if (IsRun)
                {
                    System.Threading.Thread.Sleep(Delay);
                    Hook.IO.MouseDown(VirtualKeyCode.Lbutton); 
                    Hook.IO.MouseUp(VirtualKeyCode.Lbutton);
                }
                else
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }   
    }
}
