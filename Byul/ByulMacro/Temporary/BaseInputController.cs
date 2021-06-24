using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Temporary
{
 
    public abstract class BaseInputController : IInputController
    {
        private const uint MK_LBUTTON = 0x0001;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        protected IntPtr MakeParam(int p, int p2) 
        {
            return (IntPtr)((p2 << 16) | (p & 0xffff));
        }
        public void WMMouseLeftClick(Process process, Vector2 position)
        {
            var t = PostMessage(process.MainWindowHandle, (uint)WM.WM_LBUTTONDOWN, (IntPtr)1, MakeParam((int)position.X, (int)position.Y));
            PostMessage(process.MainWindowHandle, (uint)WM.WM_LBUTTONUP, (IntPtr)0, MakeParam((int)position.X, (int)position.Y));
            Console.WriteLine("send result:" + t);

        }

        public void WMMouseRightClick(Process process, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void WMKeyboard(Process process, WM wm, VK vkMsg)
        {
            var result = PostMessage(process.MainWindowHandle, (uint)wm, (IntPtr)vkMsg, IntPtr.Zero);
            Console.WriteLine(result); 
        } 
        /// <summary>
        /// 윈도우 메세지로 보내기
        /// </summary>
        public void WMMouseClick()
        {
            
        }

    
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
