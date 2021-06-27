using ByulMacro.Core.Attributes;
using ByulMacro.Input;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByulMacro.Core.Components
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
        }
        private static bool IsStartRecord = false;
        private static System.DateTime StartTime;
        private static System.DateTime EndTIme; 
        private static CancellationTokenSource token;

        public static bool IsMouseMoveRecordable = true;


        public static bool IsStartRecording()
        {
            return IsStartRecord;
        }
        public static void StartRecord()
        {
            if (IsStartRecord == true)
                return;

            recordDatas.Clear();
            IsStartRecord = true;
            StartTime = System.DateTime.Now;
        }

        public static void StopRecord()
        { 
            if (IsStartRecord == false) 
                return;

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
            if (e.isMoveEvent)
            {
                if(Hook.IO.GetType() == typeof(Temporary.AHIInputController))
                {
                    int x = 0;
                    int y = 0;
                    if (e.x >= 1) x = 1;
                    if (e.x <= -1) x = -1;
                    if (e.y >= 1) y = 1;
                    if (e.y <= -1) y = -1;
                    Hook.IO.MoveMouse(x, y);
                    return;
                }
                else
                {
                    Hook.IO.MoveMouseDirect(e.x, e.y);
                    return;
                } 
            }
            else
            {
                VirtualKeyCode vk = VirtualKeyCode.Invalid;
            }
            if(e.mouseButton == 0)
            {
                Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Lbutton);
            }
            else if (e.mouseButton == 1)
            {
                Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Rbutton);
            }
            else if (e.mouseButton == 2)
            {
                Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Mbutton);
            }
            else if (e.mouseButton == 3)
            {
                Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Xbutton1);
            }
            else if (e.mouseButton == 4)
            {
                Hook.IO.MouseDown(LowLevelInput.Hooks.VirtualKeyCode.Xbutton2);
            }
        }

        public static void Play(System.Action finishCallback)
        {
            StartTime = DateTime.MinValue;
            EndTIme = DateTime.MinValue; 
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            token = tokenSource;

            List<RecordData> recordArrayList = new List<RecordData>();
            recordArrayList.AddRange(GetRecordDatas());
            _ = Task.Run(() =>
            { 
                if (StartTime == DateTime.MinValue)
                    StartTime = System.DateTime.Now; 
                while (recordArrayList.Count != 0)
                {
                    double cur = (System.DateTime.Now - StartTime).TotalMilliseconds;
                    if (cur >= recordArrayList[0].eventTime)
                    {
                        if (recordArrayList[0].isMouseEvent)
                        {
                            SendMouseEventRecord(recordArrayList[0].mouseEvent);
                        }
                        else
                        {
                            SendKeyboardEventRecord(recordArrayList[0].keyEvent);
                        }
                        recordArrayList.RemoveAt(0);
                    } 
                }
                finishCallback?.Invoke();
            }, tokenSource.Token);
        }


        public static void Stop(System.Action callback = null)
        {
            if (token.IsCancellationRequested)
            {
                Logger.Log("IORecord", "Already Stopped");
            }
            else
            {
                token.Cancel();  
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


        public static void OnKeyEvent(Hook.HookKeyEvent e)
        {
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
            if (IsStartRecord)
            {
                if(IsMouseMoveRecordable == false)
                {
                    if (e.isMoveEvent)
                    {
                        return;
                    }
                }
                RecordData rd = new RecordData();
                rd.order = recordDatas.Count;
                rd.isMouseEvent = e.isMoveEvent;
                rd.mouseEvent = e;
                rd.eventTime = (System.DateTime.Now - StartTime).TotalMilliseconds;
                
                recordDatas.Add(rd);
            }
        }

    }
}
