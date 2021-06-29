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
        public static bool IsStartRecord = false;
        public static bool IsPlayFlag = false;
        public static bool IsRecordMouseStartPosFlag = true;
        public static bool NoDelay = true;

        /// <summary>
        /// 마우스 움직임 자체를 녹화함
        /// true인경우 : Down, Up 이벤트로 동작하며 Move이벤트로 동작
        /// false인경우 : 마우스 클릭 위치만 기록되며, x,y위치로 마우스가 순간이동
        /// </summary>
        public static bool IsMouseMoveRecordable = true; 
        private static System.DateTime StartTime;
        private static System.DateTime EndTIme; 
        private static CancellationTokenSource token;
         

        private static bool Block = false;
        public static bool IsRecordMouseStartPos()
        {
            return IsRecordMouseStartPosFlag;
        }
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

            if (IsRecordMouseStartPosFlag && IsMouseMoveRecordable)
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
                //Hook.IO.KeyDown(e.vkCode); 
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

            Console.WriteLine(e.isMoveEvent);
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
            if (IsStartRecord || IsPlayFlag) return;
            IsPlayFlag = true;
            Logger.Log("Recoder", "Play Record..");
            StartTime = DateTime.MinValue;
            EndTIme = DateTime.MinValue; 
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            token = tokenSource; 
            List<RecordData> copyRecords = new List<RecordData>(recordDatas); 

            Console.WriteLine("record array list count : " + copyRecords.Count);
            _ = Task.Run(() =>
            { 
                if (StartTime == DateTime.MinValue)
                    StartTime = System.DateTime.Now; 
                while (copyRecords.Count != 0 && token.IsCancellationRequested == false)
                {
                    double cur = (System.DateTime.Now - StartTime).TotalMilliseconds;
                    if (NoDelay)
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

                IsPlayFlag = false;
                finishCallback?.Invoke();
                tokenSource.Cancel();
            }, tokenSource.Token);
        }


        public static void Stop(System.Action callback = null)
        {
            Logger.Log("Recoder", "Stop..");
            if(token != null && IsPlaying())
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
      
            if (IsStartRecord)
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

                    Console.WriteLine(rd.ToString());
                }
             
            }
        }

    }
}
