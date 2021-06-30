using LowLevelInput.Hooks;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace AutoHamster.Input.Component
{
    public class User32InputController : BaseInputController
    {
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(int wCode, int wMapType);
        public User32InputController()
        {
            Logger.Log(this, "Instantiate TestInputController");
        }

        public override void KeyDown(VirtualKeyCode keycode)
        {
            InputSimulator.Keyboard.KeyDown((ushort)MapVirtualKey((int)keycode, 0));
            System.Threading.Thread.Sleep(1);
        }

        public override void KeyPress(VirtualKeyCode keycode)
        {
            InputSimulator.Keyboard.KeyPress((ushort)MapVirtualKey((int)keycode, 0));
            System.Threading.Thread.Sleep(1);
        }

        public override void KeyUp(VirtualKeyCode keycode)
        {
            InputSimulator.Keyboard.KeyUp((ushort)MapVirtualKey((int)keycode, 0));
            System.Threading.Thread.Sleep(1);
        }

        public override void MouseClick(VirtualKeyCode key)
        {
            MouseDown(key);
            MouseUp(key);
        }
        public override void MouseClick(VirtualKeyCode key, Vector2 position)
        {
            MoveMouseDirect((int)position.X, (int)position.Y);
            MouseClick(key);
        }

        public override void MouseDoubleClick(VirtualKeyCode key)
        {
            if (key == VirtualKeyCode.Lbutton)
            {
                InputSimulator.Mouse.MouseDoubleClick(InputSimulator.Mouse.Buttons.Left);
            }
            else if (key == VirtualKeyCode.Rbutton)
            {
                InputSimulator.Mouse.MouseDoubleClick(InputSimulator.Mouse.Buttons.Right);
            }
            else
            {
                Logger.Error("IO Error", $"{key} is not support");
            }
        }

        public override void MouseDown(VirtualKeyCode key)
        {
            if (key == VirtualKeyCode.Lbutton)
            {
                InputSimulator.Mouse.MouseDown(InputSimulator.Mouse.Buttons.Left);
            }
            else if (key == VirtualKeyCode.Rbutton)
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
            InputSimulator.Mouse.Move(x, y);
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
