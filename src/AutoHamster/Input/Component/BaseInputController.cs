using LowLevelInput.Hooks;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace AutoHamster.Input.Component
{

    public abstract class BaseInputController : IInputController
    {
        public virtual bool IsInitialized { get; set; }
        private const uint MK_LBUTTON = 0x0001;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public IntPtr MakeParam(int p, int p2)
        {
            return (IntPtr)((p2 << 16) | (p & 0xffff));
        }
        public IntPtr MakeParam(Vector2 vec)
        {
            return (IntPtr)(((int)vec.Y << 16) | ((int)vec.X & 0xffff));
        }
        public void WMMouseLeftClick(Process process, Vector2 position)
        {
            var r1 = PostMessage(process.MainWindowHandle, (uint)WM.WM_LBUTTONDOWN, (IntPtr)1, MakeParam((int)position.X, (int)position.Y));
            var r2 = PostMessage(process.MainWindowHandle, (uint)WM.WM_LBUTTONUP, (IntPtr)0, MakeParam((int)position.X, (int)position.Y));
            if (r1 && r2) { } // Success 
            else { } // Failed
        }


        public void WMMouseLeftDrag(Process process, Vector2 downPosition, Vector2 upPosition, int whenDownThreadSleepTime)
        {
            var r1 = PostMessage(process.MainWindowHandle, (uint)WM.WM_LBUTTONDOWN, (IntPtr)1, MakeParam(downPosition));
            System.Threading.Thread.Sleep(whenDownThreadSleepTime);
            var r2 = PostMessage(process.MainWindowHandle, (uint)WM.WM_LBUTTONUP, (IntPtr)0, MakeParam(upPosition));
            if (r1 && r2) { } // Success 
            else { } // Failed
        }


        public void WMMouseRightClick(Process process, Vector2 position)
        {
            var r1 = PostMessage(process.MainWindowHandle, (uint)WM.WM_RBUTTONDOWN, (IntPtr)1, MakeParam((int)position.X, (int)position.Y));
            var r2 = PostMessage(process.MainWindowHandle, (uint)WM.WM_RBUTTONUP, (IntPtr)0, MakeParam((int)position.X, (int)position.Y));
            if (r1 && r2) { } // Success 
            else { } // Failed
        }

        public void WMMouseLeftDoubleClick(Process process, Vector2 position)
        {
            var r1 = PostMessage(process.MainWindowHandle, (uint)WM.WM_LBUTTONDBLCLK, (IntPtr)1, MakeParam((int)position.X, (int)position.Y));
            if (r1) { } // Success 
            else { } // Failed
        }

        public void WMKeyboard(Process process, WM wm, VK vkMsg)
        {
            var result = PostMessage(process.MainWindowHandle, (uint)wm, (IntPtr)vkMsg, IntPtr.Zero);
            //Console.WriteLine(result);
        }


        public void HookKeyboardEvent(VirtualKeyCode key, KeyState state, Action callback)
        {
            AutoHamster.Input.Hook.AddKeyboardEvent(key, state, callback);
        }

        public void HookMouseEvent(VirtualKeyCode key, KeyState state, Action<int, int> callback)
        {
            AutoHamster.Input.Hook.AddMouseEvent(key, state, callback);
        }

        public abstract void KeyDown(VirtualKeyCode keycode);
        public abstract void KeyPress(VirtualKeyCode keycode);
        public abstract void KeyUp(VirtualKeyCode keycode);
        public abstract void MouseClick(VirtualKeyCode key);
        public abstract void MouseClick(VirtualKeyCode key, Vector2 position);
        public abstract void MouseDoubleClick(VirtualKeyCode key); 
        public abstract void MouseDown(VirtualKeyCode key);  
        public abstract void MouseDrag(VirtualKeyCode key, Vector2 start, Vector2 end);
        public abstract void MouseUp(VirtualKeyCode key);  
        public abstract void MoveMouse(int x, int y); 
        public abstract void MoveMouseDirect(int x, int y);

        public virtual void IfNeedInitialize()
        {
            
        }
    }
}
