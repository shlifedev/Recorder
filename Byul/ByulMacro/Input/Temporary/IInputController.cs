using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Temporary
{
    /// <summary>
    /// 해당 인터페이스를 상속받아 구현시키고, 인터페이스 객체에 구현객체를 생성하여
    /// 타입에 맞춰서 사용한다.
    /// </summary>
    public interface IInputController
    {
         
        //Hook Message
        void HookMouseEvent(VirtualKeyCode key, KeyState state, System.Action<int, int> callback); 
        void HookKeyboardEvent(VirtualKeyCode key, KeyState state, System.Action callback); 

        //Hardware & Win API Message
        void KeyDown(VirtualKeyCode keycode);
        void KeyUp(VirtualKeyCode keycode);
        void KeyPress(VirtualKeyCode keycode);
        void MoveMouse(int x, int y);
        void MoveMouseDirect(int x, int y);
        void MouseDown(VirtualKeyCode key);
        void MouseUp(VirtualKeyCode key);
        void MouseClick(VirtualKeyCode key);
        void MouseDoubleClick(VirtualKeyCode key);
        void MouseDrag(VirtualKeyCode key, Vector2 start, Vector2 end);


        //Windows Message Hook
        IntPtr MakeParam(int p, int p2);
        IntPtr MakeParam(Vector2 vec);
        void WMMouseLeftClick(Process process, Vector2 position); 
        void WMMouseLeftDrag(Process process, Vector2 downPosition, Vector2 upPosition, int whenDownThreadSleepTime); 
        void WMMouseRightClick(Process process, Vector2 position); 
        void WMMouseLeftDoubleClick(Process process, Vector2 position); 
        void WMKeyboard(Process process, WM wm, VK vkMsg);


    }
}
