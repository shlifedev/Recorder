using ByulMacro.Core;
using ByulMacro.Core.Attributes;
using ByulMacro.Input;
using Coroutine;
using ImGuiNET; 
using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Linq;
using System.Windows;

namespace ByulMacro.GUI
{
    public class OverlayProcessSelector
    {
        public static OverlayProcessSelector Instance
        {
            get
            {
                if(inst == null)
                {
                    inst = new OverlayProcessSelector();
                    Hook.AddKeyboardEvent(LowLevelInput.Hooks.VirtualKeyCode.Insert, LowLevelInput.Hooks.KeyState.Down, () => {
                        OverlayProcessSelector.inst.show = !OverlayProcessSelector.inst.show;
                    });
                    return inst;
                }
                return inst;
            }
        }
        private static OverlayProcessSelector inst;
        public Process targetProcess;
        public List<Process> processList = new List<Process>();
        public bool show = false;
        private int inputX = 0, inputY = 0;
        public void ReloadProcess()
        {
            processList.AddRange(Process.GetProcesses());
            processList = (from x in processList orderby x.ProcessName select x).ToList(); 
        }

        public void AutoReloadProcess()
        {
            if (processList.Count == 0)
                ReloadProcess();
        }

        [Run]
        public static void TestInit() => GUIEventHandler.OnSelectProcess += (process) =>
        {
            OverlayProcessSelector.Instance.targetProcess = process;
        };

     
        public IEnumerator<Wait> RenderProcessSelector()
        {
            while (true)
            {
            
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender); 
                if (!show) continue;

                ImGui.Begin("프로세스 리스트 관리자", ImGuiWindowFlags.AlwaysAutoResize);
                if (targetProcess != null)
                {
                    ImGui.TextColored(new System.Numerics.Vector4(0, 1, 0, 1), targetProcess.MainWindowTitle);
                }
                else
                {
                    ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), "프로세스를 선택해주세요"); 
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



                if (ImGui.BeginMenu("Event - Click"))
                {
                    ImGui.InputInt("X 좌표", ref inputX);
                    ImGui.InputInt("Y 좌표", ref inputY);
                    if (ImGui.Button("Send Event"))
                    {
                        Hook.IO.WMMouseLeftClick(OverlayProcessSelector.Instance.targetProcess, new System.Numerics.Vector2(inputX, inputY));
                    }
                    ImGui.EndMenu();
                }
                ImGui.End();
            }
        }
    }
}