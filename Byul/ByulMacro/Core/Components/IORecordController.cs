using ByulMacro.Core.Attributes;
using ByulMacro.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Core.Components
{
    public static class IORecordController
    {
        public static List<RecordData> recordDatas = new List<RecordData>();

        public struct RecordData
        {
            public bool isMouseEvent;
            public Hook.HookMouseEvent mouseEvent;
            public Hook.HookKeyEvent keyEvent;
            public double eventTime;
            public RecordData(bool isMouseEvent, Hook.HookMouseEvent mouseEvent, Hook.HookKeyEvent keyEvent, double eventTime)
            {
                this.isMouseEvent = isMouseEvent;
                this.mouseEvent = mouseEvent;
                this.keyEvent = keyEvent;
                this.eventTime = eventTime;
            }
        }

        private static bool IsStartRecord = false;
        private static System.DateTime StartTime;
        private static System.DateTime EndTIme;
        

        public static void Start()
        {
            IsStartRecord = true;
            StartTime = System.DateTime.Now;
        }

        public static void Stop()
        {
            IsStartRecord = false;
            EndTIme = System.DateTime.Now;
        }

        public static System.DateTime[] GetStartAndEndTime() { return new DateTime[] { StartTime, EndTIme }; }
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
                RecordData rd = new RecordData();
                rd.isMouseEvent = true;
                rd.mouseEvent = e;
                rd.eventTime = (System.DateTime.Now - StartTime).TotalMilliseconds;
                recordDatas.Add(rd);
            }
        }

    }
}
