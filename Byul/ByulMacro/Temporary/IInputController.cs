using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
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
 
        /// <summary>
        /// 마우스 이벤트 후크
        /// </summary> 
        void HookMouseEvent(VirtualKeyCode key, KeyState state, System.Action<int, int> callback);
        /// <summary>
        /// 키보드 이벤트 후크
        /// </summary> 
        void HookKeyboardEvent(VirtualKeyCode key, KeyState state, System.Action callback);

        void KeyDown(VirtualKeyCode keycode);
        void KeyUp(VirtualKeyCode keycode);
        void KeyPress(VirtualKeyCode keycode);

        /// <summary>
        /// x,y 만큼 마우스 이동
        /// </summary> 
        void MoveMouse(int x, int y);

        void MouseDown(VirtualKeyCode key);
        void MouseUp(VirtualKeyCode key);
        void MouseClick(VirtualKeyCode key);
        void MouseDoubleClick(VirtualKeyCode key);
        void MouseDrag(VirtualKeyCode key, Vector2 start, Vector2 end);



    }
}
