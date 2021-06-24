using ByulMacro.Byul.Attributes;
using ByulMacro.Byul.Core;
using ByulMacro.Input;
using Coroutine;
using ImGuiNET; 
using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ByulMacro.GUI
{
    public static partial class Overlay
    {
        public static Process ProcessSelector_process;
        public static List<Process> processList = new List<Process>();
        public static void ReloadProcess()
        {
            processList.AddRange(Process.GetProcesses());
            processList = (from x in processList orderby x.ProcessName select x).ToList(); 
        }

        public static void AutoReloadProcess()
        {
            if (processList.Count == 0)
                ReloadProcess();
        }

        [Run]
        public static void TestInit() => GUIEventHandler.OnSelectProcess += (process) =>
        {
            Overlay.ProcessSelector_process = process;
        };

        static int x = 0, y = 0;
        private static IEnumerator<Wait> RenderProcessSelector()
        {
            while (true)
            {
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender);
                ImGui.Begin("debug : process list", ImGuiWindowFlags.AlwaysAutoResize);

                ImGui.SetWindowFontScale(0.85f);
                ImGui.InputInt("X", ref x);
                ImGui.InputInt("Y", ref y);

                ImGui.SetWindowFontScale(1);
                if (ImGui.Button("Send Event"))
                {
                    Console.WriteLine(ProcessSelector_process.ProcessName + "으로 메세지 보냄");
                    Hook.IO.WMMouseLeftClick(ProcessSelector_process, new System.Numerics.Vector2(x, y)); 
                }
                if (ImGui.BeginMenu("Select Process"))
                { 
                    AutoReloadProcess();
                  
                    foreach (var value in processList)
                    {
                        if (!string.IsNullOrEmpty(value.MainWindowTitle))
                        {
                            if (ImGui.MenuItem(value.MainWindowTitle))
                            { 
                                try
                                {
                                    //메인 스레드 접근
                                    Application.Current.Dispatcher.Invoke(() => { 
                                        GUIEventHandler.OnSelectProcess?.Invoke(value);
                                    }); 
                                }
                                catch(Exception e)
                                { 
                                    Console.WriteLine(e);
                                } 
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