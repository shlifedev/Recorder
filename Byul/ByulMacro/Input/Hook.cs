using ByulMacro.Temporary;
using LowLevelInput.Converters;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ByulMacro.Input
{
    public static class Hook
    {
        public static IInputController IO;
        public static Dictionary<(VirtualKeyCode key, KeyState state), System.Action<int, int>> MouseHook = new Dictionary<(VirtualKeyCode key, KeyState state), System.Action<int, int>>();
        public static Dictionary<(VirtualKeyCode key, KeyState state), System.Action> KeyboardHook = new Dictionary<(VirtualKeyCode key, KeyState state), Action>();
        public static void HookInit()
        { 
            var inputManager = new LowLevelInput.Hooks.InputManager(); 
            // you may not need those when you use InputManager
            var keyboardHook = new LowLevelKeyboardHook();
            var mouseHook = new LowLevelMouseHook(); 
            // subscribe to the events offered by InputManager
            inputManager.OnKeyboardEvent += InputManager_OnKeyboardEvent;
            inputManager.OnMouseEvent += InputManager_OnMouseEvent; 
            inputManager.Initialize();
            IO = new TestInputController();
        }



        public static void AddMouseEvent(VirtualKeyCode key, KeyState state, System.Action<int, int> callback)
        {
 
            MouseHook.Add((key, state), callback);
           
        }
        public static void AddKeyboardEvent(VirtualKeyCode key, KeyState state, System.Action callback)
        {
            KeyboardHook.Add((key, state), callback);

        }
        private static void InputManager_OnMouseEvent(VirtualKeyCode key, KeyState state, int x, int y)
        { 
            System.Action<int, int> callback;
            MouseHook.TryGetValue((key, state), out callback);
            if(callback == null)
            {
                return;
            }
            else
            {
                callback?.Invoke(x,y);
            }
            //if(key != VirtualKeyCode.Invalid)
            //Console.WriteLine($"{key}, {state}");

        }

        private static void InputManager_OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {
            System.Action callback;

            //Console.WriteLine($"{key}, {state}");
            KeyboardHook.TryGetValue((key, state), out callback);
            if (callback == null)
            {
                return;
            }
            else
            {
                callback?.Invoke();
            }
        }
    }
}
