using AutoHamster.Core.Components;
using AutoHamster.Input;
using AutoHamster.Input.Component;
using Coroutine;
using ImGuiNET;
using Microsoft.Win32;
using Pixel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows;

namespace AutoHamster.GUI
{

    public class OverlayDebugLogger
    {
        public static List<Log> logs = new List<Log>();
        public bool newLine = false;
        public bool show = false;
        public bool imguiDemo = false;
        public bool imPlotDemo = false;

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

                    //Hook.AddKeyboardEvent(LowLevelInput.Hooks.VirtualKeyCode.F2, LowLevelInput.Hooks.KeyState.Down, () =>
                    //{
                    //    if (!IORecordController.IsRecording)
                    //    {
                    //        IORecordController.StartRecord();
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        IORecordController.StopRecord();
                    //        return;
                    //    }
                    //});
                    //Hook.AddKeyboardEvent(LowLevelInput.Hooks.VirtualKeyCode.F3, LowLevelInput.Hooks.KeyState.Down, () =>
                    //{
                    //    if (!IORecordController.IsPlaying)
                    //    {
                    //        IORecordController.Play(null);
                    //        return;
                    //    }
                    //});
                    //Hook.AddKeyboardEvent(LowLevelInput.Hooks.VirtualKeyCode.F4, LowLevelInput.Hooks.KeyState.Down, () =>
                    //{
                    //    if (IORecordController.IsPlaying)
                    //    {
                    //        IORecordController.Stop(null);
                    //        return;
                    //    }
                    //});
                    return inst;
                }
                return inst;
            }
        }
        private static OverlayDebugLogger inst;  


        void DrawMenuBar()
        {
         
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Demo"))
                {
                    if (ImGui.MenuItem("ImGUIDemo"))
                    {

                        imguiDemo = !imguiDemo;


                    }
                    if (ImGui.MenuItem("ImPlotDemo"))
                    {
                        imPlotDemo = !imPlotDemo;
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
 

        void DrawImageFactoryMenu()
        {
            if (ImGui.BeginMenu("Image Factory Debug"))
            {
                foreach (var value in ImageFactory.imageContainer)
                {
                    ImGui.Text(value.Value.Tag);
                }
                ImGui.EndMenu();
            } 
        }


        private string ioRecordFileName = "";
        private void DrawIORecordMenu()
        {
           
            if (ImGui.TreeNode("IO Record Debug"))
            {

                if (ImGui.TreeNode("Save Load"))
                {
                    if (ImGui.InputText("", ref ioRecordFileName, 64))
                    {

                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Save")) { IORecordController.SaveRecord("record/" + ioRecordFileName+".json"); };

                    ImGui.SameLine();
                    if (ImGui.Button("Load")) { IORecordController.LoadRecord("record/" + ioRecordFileName + ".json"); };
                    ImGui.TreePop();

                    if (ImGui.BeginChild("##list", new Vector2(80,100)))
                    {
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("record/");
                        foreach(var value in di.GetFiles())
                        {
                            ImGui.Text(value.Name);
                        }
                        ImGui.EndChild();
                    }

                }

                ImGui.Text($"Recording : {IORecordController.IsRecording} Playing : {IORecordController.IsPlaying}");
                ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1, 1, 1, 1));  
                ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 1, 0, 1));
                if (ImGui.TreeNode("IO Record Controller Config"))
                { 
                    ImGui.PopStyleColor();
                    ImGui.Text("- Input Controller Initializer");
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(1, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1, 1, 1, 1));
                    if (ImGui.Button("AHI"))
                    {
                        Hook.IOInitialize<AHIInputController>();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("User32"))
                    {
                        Hook.IOInitialize<User32InputController>();
                    }
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();

                    ImGui.Text("- IO Record Settings"); 

                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1, 1, 1, 1));
                    ImGui.TreePush("");
                    ImGui.InputFloat("Speed Multiply", ref IORecordController.MultiplySpeed); 
                    ImGui.Checkbox("IsMouseMoveRecord", ref IORecordController.IsMouseMoveRecordable); 
                    ImGui.SameLine();
                    ImGui.Checkbox("IsKeyboardRecord", ref IORecordController.IsKeyboardRecord);

                    ImGui.Checkbox("IsRecordMouseStartPosFlag", ref IORecordController.IsRecordMouseStartPos);
                    ImGui.SameLine();
                    ImGui.Checkbox("NoDelay", ref IORecordController.IsNoDelay);
                    ImGui.TreePop(); 
                }
                ImGui.PopStyleColor();
                var rd = IORecordController.ProcessingDebugRd;

                ImGui.Text($"time|\tisMouseEvent|\torder"); 
                ImGui.Text($"{(rd.eventTime / 1000).ToString("0.00")}\t{rd.isMouseEvent}\t{rd.order}");
                ImGui.Text($"key info : {rd.keyEvent.ToString()}");

                ImGui.TextColored(new Vector4(0, 1, 0, 1), "f2/f3 (recrod&play)");
                if (!IORecordController.IsRecording)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 1, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0, 1, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0, 1, 0, 1));
                    if (ImGui.Button("[R]Start"))
                    { 
                        IORecordController.StartRecord();
                    }
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                }
                else
                {
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(1, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(1, 0, 0, 1));
                    if (ImGui.Button("[R]Stop"))
                        IORecordController.StopRecord();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                } 
                ImGui.SameLine(); 
                if (!IORecordController.IsPlaying)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1, 1, 1, 1));
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0, 0, 0, 1)); 
                    if (ImGui.Button("[P]Play"))
                    {
                        IORecordController.Play(() => {
                            Logger.Log("End!");
                        });
                    }
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                }
                else
                {
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(1, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1, 0, 0, 1));
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(1, 0, 0, 1));
                    if (ImGui.Button("[P]Stop"))
                    {
                        IORecordController.Stop();
                    }

                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                }

                 
                ImGui.Separator();
                ImGui.TextColored(new Vector4(0, 1, 0, 1), "Record Datas"); 
                ImGui.Separator(); 

                List<IORecordController.RecordData> list = IORecordController.GetRecordDatas();

 
      
                var mouseMoveEvents = (from x in list where (x.isMouseEvent == true) select x).ToList();
                if (ImGui.BeginChild("mouseMoveEvents", new Vector2(155, 100)))
                {
                    ImGui.Text("mouse move event");
                    for (int i = mouseMoveEvents.Count - 10; i < mouseMoveEvents.Count; i++)
                    {
                        if (i <= 0 || i <= 5)
                        {
                            ImGui.Text("Wait");
                            ImGui.Text("Wait");
                            ImGui.Text("Wait");
                            ImGui.Text("Wait");
                            ImGui.Text("Wait");
                            break;
                        }
            
                        IORecordController.RecordData value = mouseMoveEvents[i];
                        ImGui.Text((value.eventTime/1000).ToString("0.0") +"| " +value.mouseEvent.ToString());
                    }
                    ImGui.EndChild();
                }
                ImGui.SameLine(); 
                var mouseEvents = (from x in list where (x.isMouseEvent && x.mouseEvent.IsMoveEvent == false && x.mouseEvent.IsMoveEventDelta == false) select x).ToList();
                if (ImGui.BeginChild($"mouseEvent", new Vector2(155, 100)))
                {
                    ImGui.Text("time|state|btn|x|y");
                    for (int i = 0; i < mouseEvents.Count; i++)
                    { 
                        IORecordController.RecordData value = mouseEvents[i];
                        ImGui.Text((value.eventTime / 1000).ToString("0.0") + "| " + value.mouseEvent.ToString());
                    }
                    ImGui.SetScrollFromPosY(99999);
                    ImGui.EndChild();
                }
                var keyboardEvents = (from x in list where (x.isMouseEvent == false) select x).ToList();
                ImGui.SameLine();
                if (ImGui.BeginChild("keyboardEvents", new Vector2(155, 100)))
                {
                    ImGui.Text("time  key  state");
                    for (int i = 0; i < keyboardEvents.Count; i++)
                    { 
                        IORecordController.RecordData value = keyboardEvents[i];
                        ImGui.Text((value.eventTime / 1000).ToString("0.0") + "| " + value.keyEvent.ToString());
                    }
                    ImGui.SetScrollFromPosY(99999);
                    ImGui.EndChild();
                }
                ImGui.PopStyleColor();
            }

            ImGui.TreePop(); 
            ImGui.Separator();
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
                
                DrawImageFactoryMenu();
                DrawIORecordMenu(); 
                DrawLogs();
                 
                System.Random r = new Random();

                float[] arr = new float[] { 0.6f, 0.1f, 1.0f, 0.5f, 0.92f, 0.1f, (float)r.NextDouble() };
                ImGui.PlotLines("test", ref arr[0], arr.Length, 0, "Test PlotLine", 0, 10, new Vector2(400, 100));

                if (ImGui.Button("Clear Log"))
                { 
                    logs.Clear();
                    Logger.Log(this, "Log Cleared");
                }
                ImGui.End();
                 
                ImGui.PopFont(); 
                ImGui.PopStyleColor();


                if (imguiDemo)
                {
                    ImGui.ShowDemoWindow();
                }
                if (imPlotDemo)
                {
                    ImPlotNET.ImPlot.ShowDemoWindow();
                }
            }
        }
         
    }   
}