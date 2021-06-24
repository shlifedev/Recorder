using ByulMacro.GUI.Command;
using ByulMacro.Input;
using ClickableTransparentOverlay;
using Coroutine;
using ImGuiNET;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByulMacro.GUI
{
    public static partial class Overlay
    {
        
        private static bool showMainMenu = true;
        public static List<ICommandRenderer> test = new List<ICommandRenderer>();
  
        public static void Run()
        { 
            Task.Run(() =>
            { 
                CoroutineHandler.Start(RenderMainOverlay());
                CoroutineHandler.Start(RenderProcessSelector());
                MainLogicInputHook(); 
                ClickableTransparentOverlay.Overlay.RunInfiniteLoop();
            }); 
        }

        public static void MainLogicInputHook()
        {
            Hook.KeyboardHook.Add((VirtualKeyCode.Insert, KeyState.Up), () =>
            {
                showMainMenu = !showMainMenu;
            });
        }

        private static IEnumerator<Wait> RenderMainOverlay()
        {
            while (true)
            {
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender); 
            }
        }
    }
}