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
    public partial class Overlay
    {
        
        private static bool showMainMenu = true;
        public static List<ICommandRenderer> test = new List<ICommandRenderer>();
        public static List<Process> processList = new List<Process>();

        public static void Run()
        { 
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
                    //foreach (var commandRenderer in test)
                    //{
                    //    commandRenderer.Render();
                    //}
                    //ClickableTransparentOverlay.Overlay.AddOrGetImagePointer("test2.png", out var testptr, out var width, out var height);
                    //ImGui.Image(testptr, new Vector2(100, 100));

                    ImGui.Begin("debug : process list");

           

                    if (ImGui.BeginMenu("Select Process"))
                    {
                        Console.WriteLine("open");
                        
                        foreach (var value in processList)
                        {
                            if (!string.IsNullOrEmpty(value.MainWindowTitle))
                            {
                                if (ImGui.MenuItem(value.MainWindowTitle))
                                {
                                    Console.WriteLine(value.MainWindowTitle);
                                }
                            }
                        }
                        ImGui.EndMenu();
                    }
                    ImGui.End();
                }
            }
        }
    }
}