using AutoHamster.Core.Attributes;
using AutoHamster.Input;
using AutoHamster.Input.Component;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AutoHamster.Core.Components
{
    public static partial class IORecordController
    {
        static List<RecordData> recordDatas = new List<RecordData>(); 
        public static bool IsRecording = false;
        public static bool IsPlaying = false; 
        public static bool IsNodelay = true; 
        /// <summary>
        /// 마우스 움직임 자체를 녹화함
        /// true인경우 : Down, Up 이벤트로 동작하며 Move이벤트로 동작
        /// false인경우 : 마우스 클릭 위치만 기록되며, x,y위치로 마우스가 순간이동
        /// </summary>
        public static bool IsMouseMoveRecordable = true; 
        public static bool IsRecordMouseStartPos = true;
        private static System.DateTime StartTime;
        private static System.DateTime EndTIme; 
        private static CancellationTokenSource token; 
        public static void StartRecord()
        {
            if (IsRecording == true && IsPlaying == true)
                return;
            StackTrace st = new StackTrace(true);  
            Logger.Log("Recoder", "Start Record..");
            recordDatas.Clear();
            IsRecording = true;
            StartTime = System.DateTime.Now;

            if (IsRecordMouseStartPos && IsMouseMoveRecordable)
            {
                recordDatas.Add(new RecordData()
                {
                    eventTime = 0,
                    isMouseEvent = true,
                    mouseEvent = new Hook.HookMouseEvent()
                    {
                        controllerType = Hook.ControllerType.AHI,
                        isMoveEvent = true,
                        isMoveEventDelta = false,
                        x = Hook.mouseX,
                        y = Hook.mouseY
                    }
                }); ;
            }
           
        }

        public static void StopRecord()
        { 
            if (IsRecording == false && IsPlaying == true) 
                return;

            Logger.Log("Recoder", "End Record..");
            IsRecording = false;
            EndTIme = System.DateTime.Now;
        }

        static void SendKeyboardEventRecord(Hook.HookKeyEvent e)
        {
             if (e.state == LowLevelInput.Hooks.KeyState.Down)
            {
                Hook.IO.KeyDown(e.vkCode);
            }
            else if (e.state == LowLevelInput.Hooks.KeyState.Up)
            {
                Hook.IO.KeyUp(e.vkCode);
            }
            else
            {
                //Hook.IO.KeyDown(e.vkCode); 
            }
        }
        static void SendMouseEventRecord(Hook.HookMouseEvent e)
        {  
            if (e.isMoveEvent && e.isMoveEventDelta)
            {
                if (Hook.IO.GetType() == typeof(AHIInputController))
                {
                    Hook.IO.MoveMouse(e.x, e.y);
                    return;
                }
            }
            else if(e.isMoveEvent && e.isMoveEventDelta ==false)
            {
                Hook.IO.MoveMouseDirect(e.x, e.y);
                return;
            }
             
            if(e.mouseButton == 0 && e.isMoveEvent == false)
            {
                if (!IsMouseMoveRecordable)
                {
                    Hook.IO.MouseClick(VirtualKeyCode.Lbutton, new System.Numerics.Vector2(e.x, e.y));
                }
                else
                {
                    if (e.state == 0)
                    {
                        Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Lbutton);
                    }
                    if (e.state == 1)
                    {
                        Hook.IO.MouseUp(LowLevelInput.Hooks.VirtualKeyCode.Lbutton);
                    }
                }
            
       
            }
            else if (e.mouseButton == 1 && e.isMoveEvent == false)
            {
                if (!IsMouseMoveRecordable)
                {
                    Hook.IO.MouseClick(VirtualKeyCode.Rbutton, new System.Numerics.Vector2(e.x, e.y));
                }
                else
                {
                    if (e.state == 0)
                    {
                        Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Rbutton);
                    }
                    if (e.state == 1)
                    {
                        Hook.IO.MouseUp(LowLevelInput.Hooks.VirtualKeyCode.Rbutton);
                    }
                }
 
            }
            else if (e.mouseButton == 2 && e.isMoveEvent == false)
            {
                if (!IsMouseMoveRecordable)
                {
                    Hook.IO.MouseClick(VirtualKeyCode.Mbutton, new System.Numerics.Vector2(e.x, e.y));
                }
                else
                {
                    if (e.state == 0)
                    {
                        Hook.IO.MouseDown(VirtualKeyCode.Mbutton);
                    }
                    if (e.state == 1)
                    {
                        Hook.IO.MouseUp(VirtualKeyCode.Mbutton);
                    }
                }
            }
            else if (e.mouseButton == 3 && e.isMoveEvent == false)
            {
                if (!IsMouseMoveRecordable)
                {
                    Hook.IO.MouseClick(VirtualKeyCode.Xbutton1, new System.Numerics.Vector2(e.x, e.y));
                }
                else
                {
                    if (e.state == 0)
                    {
                        Hook.IO.MouseDown(VirtualKeyCode.Xbutton1);
                    }
                    if (e.state == 1)
                    {
                        Hook.IO.MouseUp(VirtualKeyCode.Xbutton1);
                    }
                }
            }
            else if (e.mouseButton == 4 && e.isMoveEvent == false)
            {
                if (!IsMouseMoveRecordable)
                {
                    Hook.IO.MouseClick(VirtualKeyCode.Xbutton2, new System.Numerics.Vector2(e.x, e.y));
                }
                else
                {
                    if (e.state == 0)
                    {
                        Hook.IO.MouseDown(VirtualKeyCode.Xbutton2);
                    }
                    if (e.state == 1)
                    {
                        Hook.IO.MouseUp(VirtualKeyCode.Xbutton2);
                    }
                }
            }
        }
        public static RecordData processing_debug_rd;
        public static void Play(System.Action finishCallback)
        { 
            if (IsRecording || IsPlaying) return;
            IsPlaying = true;
            Logger.Log("Recoder", "Play Record..");
            StartTime = DateTime.MinValue;
            EndTIme = DateTime.MinValue; 
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            token = tokenSource; 
            List<RecordData> copyRecords = new List<RecordData>(recordDatas);  
            _ = Task.Run(() =>
            { 
                if (StartTime == DateTime.MinValue)
                    StartTime = System.DateTime.Now; 
                while (copyRecords.Count != 0 && token.IsCancellationRequested == false)
                {
                    double cur = (System.DateTime.Now - StartTime).TotalMilliseconds * 4;
                    if (IsNodelay)
                    {
                        cur = 99999;
                    }
                    if (cur >= copyRecords[0].eventTime )
                    {
                        processing_debug_rd = copyRecords[0];
                        if (copyRecords[0].isMouseEvent)
                        {
                            SendMouseEventRecord(copyRecords[0].mouseEvent); 
                        }
                        else
                        {
                            Logger.Log("copyRecords Count : " + copyRecords.Count);
                            Logger.Log("recordDatas Count : " + recordDatas.Count);
                            SendKeyboardEventRecord(copyRecords[0].keyEvent);
                        
                        }
                        copyRecords.RemoveAt(0); 
                    }
                }


                Logger.Log("Finish Callback..");
                Logger.Log("copyRecords Count : " + copyRecords.Count);
                Logger.Log("recordDatas Count : " + recordDatas.Count);

                IsPlaying = false;
                finishCallback?.Invoke();
                tokenSource.Cancel();
            }, tokenSource.Token);
        }


        public static void Stop(System.Action callback = null)
        {
            Logger.Log("Recoder", "Stop..");
            if(token != null && IsPlaying)
            {
                token.Cancel();
                IsPlaying = false;
                callback?.Invoke();
            }
          
        }
        public static System.DateTime[] GetStartAndEndTime() 
        {
            return new DateTime[] { StartTime, EndTIme };
        }

        public static List<RecordData> GetRecordDatas()
        {
            return recordDatas;
        }
        [Run]
        public static void RegEvent()
        {
            Hook.onKeyboardEvent += OnKeyEvent;
            Hook.onMouseEvent += OnMouseEvent;
        } 

        public static double GetEventTime()
        {
            return (System.DateTime.Now - StartTime).TotalMilliseconds;
        }
        public static void OnKeyEvent(Hook.HookKeyEvent e)
        {
            if (e.vkCode == VirtualKeyCode.F2 || e.vkCode == VirtualKeyCode.F3) return;

        
            if (IsRecording)
            {
                RecordData rd = new RecordData();
                rd.order = recordDatas.Count;
                rd.isMouseEvent = false;
                rd.keyEvent = e;
                rd.eventTime = (System.DateTime.Now - StartTime).TotalMilliseconds;
                recordDatas.Add(rd);
            }

        }
        public static void OnMouseEvent(Hook.HookMouseEvent e)
        {
      
            if (IsRecording)
            { 
                if(IsMouseMoveRecordable == false)
                {
                    if (e.isMoveEvent || e.isMoveEventDelta)
                    {
                        return;
                    }
                } 
                if (e.isMoveEvent)
                {
                    RecordData rd = new RecordData();
                    rd.order = recordDatas.Count;
                    rd.isMouseEvent = true;
                    rd.mouseEvent = e; 
                    rd.eventTime = (System.DateTime.Now - StartTime).TotalMilliseconds;
                    recordDatas.Add(rd);
                }
                else
                { 
                    RecordData rd = new RecordData();
                    rd.order = recordDatas.Count;
                    rd.isMouseEvent = true;
                    rd.mouseEvent = e;
                    rd.eventTime = (System.DateTime.Now - StartTime).TotalMilliseconds;
                    recordDatas.Add(rd); 
                }
             
            }
        }

    }
}
