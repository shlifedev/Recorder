using ByulMacro.GUI.Command;
using ByulMacro.Input;
using ClickableTransparentOverlay;
using Coroutine;
using ImGuiNET;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByulMacro.GUI
{
    public partial class Overlay
    {
        
        private static bool showMainMenu = true;
        public static List<ICommandRenderer> test = new List<ICommandRenderer>();
        public static void Run()
        {
            test.Add(new CommandRenderer());
            test.Add(new CommandRenderer() { index = 1});
            test.Add(new CommandRenderer() { index = 2});
            Task.Run(() =>
            {
                CoroutineHandler.Start(MainLogic());
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

        private static IEnumerator<Wait> MainLogic()
        {
            while (true)
            {
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender);
                if (showMainMenu)
                {
                    foreach(var commandRenderer in test)
                    {
                        commandRenderer.Render();
                    } 
                }
            }
        }
    }
}