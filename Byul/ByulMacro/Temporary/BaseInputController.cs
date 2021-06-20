using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Temporary
{
    public abstract class BaseInputController : IInputController
    {
        public void HookKeyboardEvent(VirtualKeyCode key, KeyState state, Action callback)
        {
            ByulMacro.Input.Hook.AddKeyboardEvent(key, state, callback);
        }

        public void HookMouseEvent(VirtualKeyCode key, KeyState state, Action<int, int> callback)
        {
            ByulMacro.Input.Hook.AddMouseEvent(key, state, callback);
        }

        public abstract void KeyDown(VirtualKeyCode keycode);
        public abstract void KeyPress(VirtualKeyCode keycode);
        public abstract void KeyUp(VirtualKeyCode keycode);
        public abstract void MouseClick(VirtualKeyCode key);
        public abstract void MouseDoubleClick(VirtualKeyCode key);
        public abstract void MouseDown(VirtualKeyCode key);
        public abstract void MouseDrag(VirtualKeyCode key, Vector2 start, Vector2 end);
        public abstract void MouseUp(VirtualKeyCode key);
        public abstract void MoveMouse(int x, int y);


    }
}
