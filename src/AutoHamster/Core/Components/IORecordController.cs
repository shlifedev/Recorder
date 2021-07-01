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
        public static RecordData ProcessingDebugRd;
        static readonly List<RecordData> RecordDatas = new List<RecordData>();
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
        private static System.DateTime _startTime;
        private static System.DateTime _endTIme;
        private static CancellationTokenSource _token;
        public static float MultiplySpeed = 1.0f;
        public static void StartRecord()
        {
            if (IsRecording == true && IsPlaying == true)
                return;
            StackTrace st = new StackTrace(true);
            Logger.Log("Recoder", "Start Record..");
            RecordDatas.Clear();
            IsRecording = true;
            _startTime = System.DateTime.Now;

            if (IsRecordMouseStartPos && IsMouseMoveRecordable)
            {
                RecordDatas.Add(new RecordData()
                {
                    eventTime = 0,
                    isMouseEvent = true,
                    mouseEvent = new Hook.HookMouseEvent()
                    {
                        ControllerType = Hook.ControllerType.AHI,
                        IsMoveEvent = true,
                        IsMoveEventDelta = false,
                        X = Hook.mouseX,
                        Y = Hook.mouseY
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
            _endTIme = DateTime.Now;
        }

        static void SendKeyboardEventRecord(Hook.HookKeyEvent e)
        {
            if (e.state == KeyState.Down)
            {
                Hook.IO.KeyDown(e.vkCode);
            }
            else if (e.state == KeyState.Up)
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
            if (e.IsMoveEvent && e.IsMoveEventDelta)
            {
                if (Hook.IO.GetType() == typeof(AHIInputController))
                {
                    Hook.IO.MoveMouse(e.X, e.Y);
                    return;
                }
            }
            else if (e.IsMoveEvent)
            {
                Hook.IO.MoveMouseDirect(e.X, e.Y);
                return;
            }

            VirtualKeyCode vk = VirtualKeyCode.Hotkey;
            vk = (e.MouseButton == 0) ? VirtualKeyCode.Lbutton :
                 (e.MouseButton == 1) ? VirtualKeyCode.Rbutton :
                 (e.MouseButton == 2) ? VirtualKeyCode.Mbutton :
                 (e.MouseButton == 3) ? VirtualKeyCode.Xbutton1 : VirtualKeyCode.Xbutton2;



            if (!IsMouseMoveRecordable)
            {
                Hook.IO.MouseClick(vk, new System.Numerics.Vector2(e.X, e.Y));
            }
            else
            {
                if (e.State == 0)
                {
                    Hook.IO.MouseDown(vk);
                }
                if (e.State == 1)
                {
                    Hook.IO.MouseUp(vk);
                }
            }
        }
        public static void Play(System.Action finishCallback)
        {
            if (IsRecording || IsPlaying) return;
            IsPlaying = true;
            Logger.Log("Recoder", "Play Record..");
            _startTime = DateTime.MinValue;
            _endTIme = DateTime.MinValue;
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            _token = tokenSource;
            List<RecordData> copyRecords = new List<RecordData>(RecordDatas);
            _ = Task.Run(() =>
            {
                if (_startTime == DateTime.MinValue)
                    _startTime = System.DateTime.Now;
                while (copyRecords.Count != 0 && _token.IsCancellationRequested == false)
                {
                    double cur = (System.DateTime.Now - _startTime).TotalMilliseconds * MultiplySpeed;
                    if (IsNodelay)
                    {
                        cur = 99999;
                    }
                    if (cur >= copyRecords[0].eventTime)
                    {
                        ProcessingDebugRd = copyRecords[0];
                        if (copyRecords[0].isMouseEvent)
                        {
                            SendMouseEventRecord(copyRecords[0].mouseEvent);
                        }
                        else
                        {
                            Logger.Log("copyRecords Count : " + copyRecords.Count);
                            Logger.Log("recordDatas Count : " + RecordDatas.Count);
                            SendKeyboardEventRecord(copyRecords[0].keyEvent);

                        }
                        copyRecords.RemoveAt(0);
                    }
                }


                Logger.Log("Finish Callback..");
                Logger.Log("copyRecords Count : " + copyRecords.Count);
                Logger.Log("recordDatas Count : " + RecordDatas.Count);

                IsPlaying = false;
                finishCallback?.Invoke();
                tokenSource.Cancel();
            }, tokenSource.Token);
        }


        public static void Stop(System.Action callback = null)
        {
            Logger.Log("Recoder", "Stop..");
            if (_token != null && IsPlaying)
            {
                _token.Cancel();
                IsPlaying = false;
                callback?.Invoke();
            }

        }
        public static System.DateTime[] GetStartAndEndTime()
        {
            return new DateTime[] { _startTime, _endTIme };
        }

        public static List<RecordData> GetRecordDatas()
        {
            return RecordDatas;
        }
        [Run]
        public static void RegEvent()
        {
            Hook.onMouseEvent += OnMouseEvent;
            Hook.onKeyboardEvent += OnKeyEvent;
        }

        public static double GetEventTime()
        {
            return (System.DateTime.Now - _startTime).TotalMilliseconds;
        }

        private static void OnKeyEvent(Hook.HookKeyEvent e)
        {
            if (e.vkCode == VirtualKeyCode.F2 || e.vkCode == VirtualKeyCode.F3) return;


            if (IsRecording)
            {
                RecordData rd = new RecordData();
                rd.order = RecordDatas.Count;
                rd.isMouseEvent = false;
                rd.keyEvent = e;
                rd.eventTime = (System.DateTime.Now - _startTime).TotalMilliseconds;
                RecordDatas.Add(rd);
            }

        }

        private static void OnMouseEvent(Hook.HookMouseEvent e)
        {

            if (IsRecording)
            {
                if (IsMouseMoveRecordable == false)
                {
                    if (e.IsMoveEvent || e.IsMoveEventDelta)
                    {
                        return;
                    }
                }
                if (e.IsMoveEvent)
                {
                    RecordData rd = new RecordData();
                    rd.order = RecordDatas.Count;
                    rd.isMouseEvent = true;
                    rd.mouseEvent = e;
                    rd.eventTime = (System.DateTime.Now - _startTime).TotalMilliseconds;
                    RecordDatas.Add(rd);
                }
                else
                {
                    RecordData rd = new RecordData();
                    rd.order = RecordDatas.Count;
                    rd.isMouseEvent = true;
                    rd.mouseEvent = e;
                    rd.eventTime = (System.DateTime.Now - _startTime).TotalMilliseconds;
                    RecordDatas.Add(rd);
                }

            }
        }

    }
}