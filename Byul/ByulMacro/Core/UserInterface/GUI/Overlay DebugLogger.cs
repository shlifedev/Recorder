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
public enum LogType
{
    Debug, Error, Warning
}
public struct Log
{
    public DateTime TimeStamp;
    public LogType Type;
    public string Tag;
    public string Msg;

    public Log(LogType type, string tag, string msg)
    {
        TimeStamp = DateTime.Now;
        Type = type;
        Tag = tag;
        Msg = msg;
    }
    public string GetMsgWithoutTag(bool datetime = true)
    {
        if (datetime)
        {
            return $"[{TimeStamp.ToString("hh:mm:ss")}] {Msg}";
        }
        else
        {
            return $"{Msg}";
        }
    }
    public string GetMsgWithTag(bool datetime = true)
    {
        if (datetime)
        {
            return $"[{TimeStamp.ToString("hh:mm:ss")}] [{Tag}] {Msg}";
        }
        else
        {
            return $"[{Tag}] {Msg}";
        } 
    }
}
public class Logger
{
 
    public static Action<Log> onLogged;
    public static List<Log> logs = new List<Log>();
    public static void Log(string tag, string msg)
    {
        Log log = new Log(LogType.Debug, tag, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Warning(string tag, string msg)
    {
        Log log = new Log(LogType.Warning, tag, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Error(string tag, string msg)
    {
        Log log = new Log(LogType.Error, tag, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }


    public static void Log(string msg)
    {
        Log log = new Log(LogType.Debug, "Log", msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Warning(string msg)
    {
        Log log = new Log(LogType.Warning, "Warning", msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Error(string msg)
    {
        Log log = new Log(LogType.Error, "Error", msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }



    public static void Log(this object tag, string msg)
    {
        Log log = new Log(LogType.Debug, tag.GetType().Name, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Warning(this object tag, string msg)
    {
        Log log = new Log(LogType.Warning, tag.GetType().Name, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Error(this object tag, string msg)
    {
        Log log = new Log(LogType.Error, tag.GetType().Name, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
}
namespace ByulMacro.GUI
{
 
    public class OverlayDebugLogger
    {
        public static List<Log> logs = new List<Log>();
        public bool newLine = false;
        public bool show = false;
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
        public IEnumerator<Wait> RenderDebugger()
        {
            while (true)
            { 
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender);
                if (!show) continue;

                ImGui.PushFont(FontPointer.FontFactory["default"]);
                ImGui.Begin("Logger", ImGuiWindowFlags.AlwaysAutoResize);
                if (ImGui.Button("Clear Log")) {
                    logs.Clear();
                } 
                if (ImGui.BeginChild("##scrolling", new Vector2(400, 200), false, 0))
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
                        ImGui.SetScrollHereY(1.0f);
                    }
                    ImGui.EndChild();
                } 
                ImGui.End();
                ImGui.PopFont();
            }
        }
    }
}