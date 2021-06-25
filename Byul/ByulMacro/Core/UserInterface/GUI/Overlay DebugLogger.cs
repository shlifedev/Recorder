using ByulMacro.Input;
using Coroutine;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
namespace ByulMacro.GUI
{

    public class OverlayDebugLogger
    {
        public static List<Log> logs = new List<Log>();
        public bool newLine = false;
        public bool show = false;

        public static void Init()
        {
            var x = OverlayDebugLogger.Instance; // run single ton
        }
        public static OverlayDebugLogger Instance
        {
            get
            {
                if(inst == null)
                { 
                    inst = new OverlayDebugLogger();
                    Logger.onLogged += (x) =>
                    {
                        logs.Add(x);
                        inst.newLine = true;
                    };
                    Hook.AddKeyboardCombo(LowLevelInput.Hooks.VirtualKeyCode.Lcontrol, LowLevelInput.Hooks.VirtualKeyCode.Two, () => { 
                        OverlayDebugLogger.inst.show = !OverlayDebugLogger.inst.show;
                    });
                    return inst;
                }
                return inst;
            }
        }
        private static OverlayDebugLogger inst;

        private int x, y;
        public IEnumerator<Wait> RenderDebugger()
        {
            while (true)
            { 
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender);
                if (!show) continue;

                ImGui.PushFont(FontPointer.FontFactory["default"]); 
                ImGui.Begin("Logger", ImGuiWindowFlags.AlwaysAutoResize);
 
                if(ImGui.BeginMenu("Input Debugger"))
                {
                    ImGui.TextColored(new Vector4(0, 1, 0, 1), "- IO Info");
                    ImGui.Text($"MPos : {Hook.mouseX} {Hook.mouseY}");
                    ImGui.Text($"Current IO : {Hook.IO.GetType().Name}");


                    ImGui.TextColored(new Vector4(0, 1, 0, 1), "- Input Controller");
                    if (ImGui.Button("Use Ahi")) { Hook.IO = new Temporary.AHIInputController(); Hook.IO.IfNeedInitialize(); }
                    ImGui.SameLine();
                    if (ImGui.Button("Use Test")) { Hook.IO = new Temporary.User32InputController(); }

                    ImGui.TextColored(new Vector4(0, 1, 0, 1), "- Input Test");
                    ImGui.InputInt2("X", ref x); 
                    if (ImGui.Button($"Absolute Move {x},{y}"))
                    {
                        Hook.IO = new Temporary.AHIInputController(); 
                        Hook.IO.IfNeedInitialize(); 
                        Hook.IO.MoveMouseDirect(x, y);

                        System.Threading.Thread.Sleep(500);
                        Hook.IO = new Temporary.User32InputController();

                        Hook.IO.MoveMouseDirect(x, y);


                    }
                    if (ImGui.Button($"Releative Move {x},{y}"))
                    {
                        Hook.IO.MoveMouse(x, y);
                    }
                    ImGui.EndMenu();
                }
                

                 
                if (ImGui.BeginChild("##scrolling", new Vector2(600, 200), false, 0))
                {
                    foreach(var log in logs)
                    {
                        if (log.Type == LogType.Debug)
                        {
                            ImGui.TextColored(new Vector4(0, 1, 0, 1), "[" + log.TimeStamp.ToString("hh:mm:ss") + "]");
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0, 1, 0, 1), "[" + log.Tag + "]");
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(1, 1, 1, 1), log.GetMsgWithoutTag(false));
                        }
                        if (log.Type == LogType.Error)
                        {
                            ImGui.TextColored(new Vector4(1, 0, 0, 1), "[" + log.TimeStamp.ToString("hh:mm:ss") + "]");
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(1, 0, 0, 1), "[" + log.Tag + "]");
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(1, 1, 1, 1), log.GetMsgWithoutTag(false));
                        }
                        if (log.Type == LogType.Warning)
                        {
                            ImGui.TextColored(new Vector4(1, 1, 0, 1), "[" + log.TimeStamp.ToString("hh:mm:ss") + "]");
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(1, 1, 0, 1), "[" + log.Tag + "]");
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(1, 1, 1, 1), log.GetMsgWithoutTag(false));
                        }
                    }

                    if (OverlayDebugLogger.inst.newLine)
                    {
                        OverlayDebugLogger.inst.newLine = false;
                        Console.WriteLine("newline");
                        ImGui.SetScrollHereY(1);
                        ImGui.SetScrollFromPosY(100000);
                    } 
                    ImGui.EndChild();
                }


                if (ImGui.Button("Clear Log"))
                {
                    logs.Clear();
                }
                ImGui.End();
                ImGui.PopFont();
            }
        }
    }
}