using AutoHamster.Core.Attributes;
using AutoHamster.Input;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoHamster.Core.Components
{
    public static class IORecordController
    {
        static List<RecordData> recordDatas = new List<RecordData>();

        public struct RecordData
        {
            public int order;
            public bool isMouseEvent;
            public Hook.HookMouseEvent mouseEvent;
            public Hook.HookKeyEvent keyEvent;
            public double eventTime;
            public RecordData(int order, bool isMouseEvent, Hook.HookMouseEvent mouseEvent, Hook.HookKeyEvent keyEvent, double eventTime)
            {
                this.order = order; 
                this.isMouseEvent = isMouseEvent;
                this.mouseEvent = mouseEvent;
                this.keyEvent = keyEvent;
                this.eventTime = eventTime;
            }

            public override string ToString()
            {
                return $"isMouseEvent\tmouseEvent\tkeyEvent\teventTime\n{isMouseEvent}\t{mouseEvent.ToString()}\t{keyEvent.ToString()}\t{eventTime}";
            }
        }
        private static bool IsStartRecord = false;
        private static bool IsPlayFlag = false;
        private static bool IsRecordMouseStartPos = true;
        private static System.DateTime StartTime;
        private static System.DateTime EndTIme; 
        private static CancellationTokenSource token;

        public static bool IsMouseMoveRecordable = true;

        private static bool Block = false;
       
        public static bool IsStartRecording()
        {
            return IsStartRecord;
        }
        public static bool IsPlaying()
        {
            return IsPlayFlag;
        }
        public static void StartRecord()
        {
            if (IsStartRecord == true && IsPlayFlag == true)
                return;
            StackTrace st = new StackTrace(true);

            Console.WriteLine(st.ToString());
            
            Logger.Log("Recoder", "Start Record..");
            recordDatas.Clear();
            IsStartRecord = true;
            StartTime = System.DateTime.Now;

            if (IsRecordMouseStartPos)
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
            if (IsStartRecord == false && IsPlayFlag == true) 
                return;

            Logger.Log("Recoder", "End Record..");
            IsStartRecord = false;
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
                throw new Exception("not support");
            }
        }
        static void SendMouseEventRecord(Hook.HookMouseEvent e)
        {  
            if (e.isMoveEvent && e.isMoveEventDelta)
            {
                if(Hook.IO.GetType() == typeof(Component.AHIInputController))
                {
                    if(e.controllerType == Hook.ControllerType.AHI)
                    {
                        Hook.IO.MoveMouse(e.x, e.y);
                    } 
                    return;
                }
                else
                {
                    Hook.IO.MoveMouseDirect(e.x, e.y);
                    return;
                } 
            }
            else if(e.isMoveEvent && e.isMoveEventDelta ==false)
            {
                Hook.IO.MoveMouseDirect(e.x, e.y);
            }
            if(e.mouseButton == 0)
            {
                if(e.state == 0)
                {
                    Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Lbutton);
                }
                if(e.state == 1)
                {
                    Hook.IO.MouseUp(LowLevelInput.Hooks.VirtualKeyCode.Lbutton);
                }
       
            }
            else if (e.mouseButton == 1)
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
            else if (e.mouseButton == 2)
            {
                if (e.state == 0)
                {
                    Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Mbutton);
                }
                if (e.state == 1)
                {
                    Hook.IO.MouseUp(LowLevelInput.Hooks.VirtualKeyCode.Mbutton);
                }
            }
            else if (e.mouseButton == 3)
            {
                if (e.state == 0)
                {
                    Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Xbutton1);
                }
                if (e.state == 1)
                {
                    Hook.IO.MouseUp(LowLevelInput.Hooks.VirtualKeyCode.Xbutton1);
                }
            }
            else if (e.mouseButton == 4)
            {
                if (e.state == 0)
                {
                    Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Xbutton2);
                }
                if (e.state == 1)
                {
                    Hook.IO.MouseUp(LowLevelInput.Hooks.VirtualKeyCode.Xbutton2);
                }
            }
        }

        public static void Play(System.Action finishCallback)
        { 
            if (IsStartRecord || IsPlayFlag) return; 
            Logger.Log("Recoder", "Play Record..");
            StartTime = DateTime.MinValue;
            EndTIme = DateTime.MinValue; 
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            token = tokenSource;
            IsPlayFlag = true;
            List<RecordData> copyRecords = new List<RecordData>(recordDatas); 

            Console.WriteLine("record array list count : " + copyRecords.Count);
            _ = Task.Run(() =>
            { 
                if (StartTime == DateTime.MinValue)
                    StartTime = System.DateTime.Now; 
                while (copyRecords.Count != 0)
                {
                    double cur = (System.DateTime.Now - StartTime).TotalMilliseconds;
                    if (cur >= copyRecords[0].eventTime)
                    {
                        if (copyRecords[0].isMouseEvent)
                        {
                            SendMouseEventRecord(copyRecords[0].mouseEvent); 
                        }
                        else
                        {
                            SendKeyboardEventRecord(copyRecords[0].keyEvent);
                            Logger.Log("copyRecords Count : " + copyRecords.Count);
                            Logger.Log("recordDatas Count : " + recordDatas.Count);
                        }
                        copyRecords.RemoveAt(0); 
                    } 
                }


                Logger.Log("Finish Callback..");
                Logger.Log("copyRecords Count : " + copyRecords.Count);
                Logger.Log("recordDatas Count : " + recordDatas.Count);

                IsPlayFlag = false;
                finishCallback?.Invoke();
                tokenSource.Cancel();
            }, tokenSource.Token);
        }


        public static void Stop(System.Action callback = null)
        {
            Logger.Log("Recoder", "Stop Record..");
            if (token.IsCancellationRequested)
            { 
                Logger.Log("IORecord", "Already Stopped");
            }
            else
            {
                token.Cancel();
                IsPlayFlag = false;
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

            if (Block)
            {
                Block = false;
                return;
            }
            if (IsStartRecord)
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
            if (Block)
            {
                Block = false;
                return;
            }
            if (IsStartRecord)
            { 
                if(IsMouseMoveRecordable == false)
                {
                    if (e.isMoveEvent)
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

                    Console.WriteLine(rd.ToString());
                }
             
            }
        }

    }
}
