using AutoHamster.Core.Components;
using AutoHamster.Input;
using AutoHamster.Component;
using Coroutine;
using ImGuiNET;
using Pixel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
namespace AutoHamster.GUI
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
        private int inputX, inputY;



        void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Utility"))
                {
                    if (ImGui.MenuItem("KBoost"))
                    {

                        try
                        {
                            Process proc = Process.Start(@"H:\유틸리티\KBoost.exe");
                        }
                        catch
                        {
                            Logger.Error(this, "Cannot Found");
                        }


                    }
                    if (ImGui.MenuItem("Goodbye DPI"))
                    {
                        try
                        {
                            Process proc = Process.Start(@"H:\유틸리티\goodbyedpi-0.1.6\x86_64\goodbyedpi.exe");
                        }
                        catch
                        {
                            Logger.Error(this, "Cannot Found");
                        }


                    }
                    if (ImGui.MenuItem("Animation Folder"))
                    {
                        try
                        {
                            Process proc = Process.Start(@"H:\애니");
                        }
                        catch
                        {
                            Logger.Error(this, "Cannot Found");
                        }


                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }
        void DrawIODebuggerMenu()
        {
            if (ImGui.BeginMenu("Input Debugger"))
            {
                ImGui.TextColored(new Vector4(0, 1, 0, 1), "- IO Info");
                ImGui.Text($"MPos : {Hook.mouseX} {Hook.mouseY}");
                ImGui.Text($"Current IO : {Hook.IO.GetType().Name}");
                ImGui.TextColored(new Vector4(0, 1, 0, 1), "- Input Controller");
                if (ImGui.Button("Use Ahi"))
                {
                    Hook.IOInitialize<AHIInputController>();
                }
                ImGui.SameLine();
                if (ImGui.Button("Use Test"))
                {
                    Hook.IOInitialize<User32InputController>();
                }

                ImGui.TextColored(new Vector4(0, 1, 0, 1), "- Input Test");
                ImGui.InputInt2("X", ref inputX);
                if (ImGui.Button($"Absolute Move {inputX},{inputY}"))
                {
                    Hook.IOInitialize<AHIInputController>();
                    Hook.IO.IfNeedInitialize();
                    Hook.IO.MoveMouseDirect(inputX, inputY);
                    System.Threading.Thread.Sleep(500);
                    Hook.IOInitialize<User32InputController>();
                    Hook.IO.MoveMouseDirect(inputX, inputY);
                }
                if (ImGui.Button($"Releative Move {inputX},{inputY}"))
                {
                    Hook.IO.MoveMouse(inputX, inputY);
                }
                if (ImGui.Button($"Keyboard Test"))
                {
                    Hook.IOInitialize<AHIInputController>();
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.A);
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.B);
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.C);
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.D);
                    Hook.IOInitialize<User32InputController>();
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.A);
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.B);
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.C);
                    Hook.IO.KeyPress(LowLevelInput.Hooks.VirtualKeyCode.D);
                }
                ImGui.EndMenu();
            }
        }

        void DrawImageFactoryMenu()
        {
            if (ImGui.BeginMenu("Image Factory Debug"))
            {
                foreach (var value in ImageFactory.imageContainer)
                {
                    ImGui.Text(value.Value.tag);
                }
                ImGui.EndMenu();
            }
        }

        private void DrawIORecordMenu()
        {
            if (ImGui.BeginMenu("IO Record Debug"))
            {
                if (ImGui.Button("[R]Start"))
                {
                    IORecordController.StartRecord();
                }
                ImGui.SameLine();
                if (ImGui.Button("[R]Stop"))
                {
                    IORecordController.StopRecord();
                }
                ImGui.SameLine();
                if (ImGui.Button("[P]Play"))
                {
                    IORecordController.Play(() => {
                        Logger.Log("End!");
                    });
                }
                ImGui.SameLine();
                if (ImGui.Button("[P]Stop"))
                {
                    IORecordController.Stop();
                }
                ImGui.LabelText("hello", "hello");
                foreach (var value in ImageFactory.imageContainer)
                {
                    ImGui.Text(value.Value.tag);
                }
                ImGui.EndMenu();
            }

        }

        void DrawLogs()
        {
            if (ImGui.BeginChild("##Logs", new Vector2(600, 200), false, 0))
            {
                foreach (var log in logs)
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

        }
        public IEnumerator<Wait> RenderDebugger()
        {
            while (true)
            { 
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender);
                if (!show) continue;


          

                ImGui.PushStyleColor(ImGuiCol.TitleBgActive, new Vector4(1f, 0, 0, 1));
                ImGui.PushFont(FontPointer.FontFactory["default"]); 
                ImGui.Begin("Logger", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoMove);
                ImGui.SetWindowPos(new Vector2(0, 0)); 
                DrawMenuBar();
                ImGui.PushStyleVar(ImGuiStyleVar.WindowTitleAlign, new Vector2(0.5f,0.5f)); 
                DrawIODebuggerMenu();
                DrawImageFactoryMenu();
                DrawIORecordMenu(); 
                DrawLogs();


                System.Random r = new Random();

                float[] arr = new float[] { 0.6f, 0.1f, 1.0f, 0.5f, 0.92f, 0.1f, (float)r.NextDouble() };
                ImGui.PlotLines("test", ref arr[0], arr.Length, 0, "ZZ", 0, 10, new Vector2(400, 100));

                if (ImGui.Button("Clear Log"))
                { 
                    logs.Clear();
                    Logger.Log(this, "Log Cleared");
                }
                ImGui.End();
                 
                ImGui.PopFont(); 
                ImGui.PopStyleColor(); 
            }
        }
         
    }
}