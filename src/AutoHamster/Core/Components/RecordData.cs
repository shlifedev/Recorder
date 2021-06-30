using AutoHamster.Input;

namespace AutoHamster.Core.Components
{
    public static partial class IORecordController
    {
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

    }
}
