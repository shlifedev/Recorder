using AutoHotInterception;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Temporary
{
    public class TestInputController : BaseInputController
    {
   
        public override void KeyDown(VirtualKeyCode keycode)
        {
            throw new NotImplementedException();
        }

        public override void KeyPress(VirtualKeyCode keycode)
        {
            throw new NotImplementedException();
        }

        public override void KeyUp(VirtualKeyCode keycode)
        {
            throw new NotImplementedException();
        }

        public override void MouseClick(VirtualKeyCode key)
        {
            throw new NotImplementedException();
        }

        public override void MouseDoubleClick(VirtualKeyCode key)
        {
            throw new NotImplementedException();
        }

        public override void MouseDown(VirtualKeyCode key)
        {
            if(key == VirtualKeyCode.Lbutton)
            {
                InputSimulator.Mouse.MouseDown(InputSimulator.Mouse.Buttons.Left);
            }
            else if(key == VirtualKeyCode.Rbutton)
            {
                InputSimulator.Mouse.MouseDown(InputSimulator.Mouse.Buttons.Right);
            }
            else
            {
                Logger.Error("IO Error", $"{key} is not support");
            }
        }

        public override void MouseDrag(VirtualKeyCode key, Vector2 start, Vector2 end)
        {
            throw new NotImplementedException();
        }

        public override void MouseUp(VirtualKeyCode key)
        {
            if (key == VirtualKeyCode.Lbutton)
            {
                InputSimulator.Mouse.MouseUp(InputSimulator.Mouse.Buttons.Left);
            }
            else if (key == VirtualKeyCode.Rbutton)
            {
                InputSimulator.Mouse.MouseUp(InputSimulator.Mouse.Buttons.Right);
            }
            else
            {
                Logger.Error("IO Error", $"{key} is not support");
            }
        }

        public override void MoveMouse(int x, int y)
        {
            InputSimulator.Mouse.MoveDirect(x, y);
        }

        public override void IfNeedInitialize()
        {

        }
        public override void MoveMouseDirect(int x, int y)
        {
            InputSimulator.Mouse.MoveDirect(x, y);
        }
    }
}
