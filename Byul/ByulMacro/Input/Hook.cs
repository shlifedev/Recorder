using AutoHotInterception;
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
        static IInputController io;
        public static Dictionary<(VirtualKeyCode key, KeyState state), System.Action<int, int>> MouseHook = new Dictionary<(VirtualKeyCode key, KeyState state), System.Action<int, int>>();
        public static Dictionary<(VirtualKeyCode key, KeyState state), System.Action> KeyboardHook = new Dictionary<(VirtualKeyCode key, KeyState state), Action>();
        public static Dictionary<(VirtualKeyCode k1, VirtualKeyCode k2), System.Action> KeyComboHook = new Dictionary<(VirtualKeyCode k1, VirtualKeyCode k2), Action>();
        private static bool Logging = false;
        public static int mouseX, mouseY;
        public static LowLevelInput.Hooks.InputManager inputManager;
         
        static VirtualKeyCode _ComboStartKey = VirtualKeyCode.Invalid;

         

        public static IInputController IO { get => io; }

        public static void IOInitialize<T>() where T : IInputController
        { 
            var type = typeof(T);
            if (type == typeof(User32InputController)) 
                io = new User32InputController(); 
            else if (type == typeof(AHIInputController)) 
                io = new AHIInputController();
            else
            {
                throw new Exception("Not Support " + type.Name);
            }
        }

        public static void HookInit(System.Action onInited = null)
        { 
            inputManager = new LowLevelInput.Hooks.InputManager(); 
            // you may not need those when you use InputManager
            var keyboardHook = new LowLevelKeyboardHook();
            var mouseHook = new LowLevelMouseHook();
            // subscribe to the events offered by InputManager
            inputManager.OnKeyboardEvent += InputManager_OnKeyboardEvent;
            inputManager.OnMouseEvent += InputManager_OnMouseEvent; 
            inputManager.Initialize();
            IOInitialize<AHIInputController>();
            IO.IfNeedInitialize();
            onInited?.Invoke();

            Hook.AddMouseEvent(VirtualKeyCode.Invalid, KeyState.None, (x, y) => {
                mouseX = x;
                mouseY = y; 
            });

        }
 

        public static void AddKeyboardCombo(VirtualKeyCode k1, VirtualKeyCode k2, System.Action callback)
        {
            if (KeyComboHook.ContainsKey((k1, k2)))
            {
                Logger.Log("Add Keyboard Combo", $"{k1}+{k2}");
                KeyComboHook[(k1, k2)] += callback;
            }
            else
            {
                Logger.Log("Add New Keyboard Combo", $"{k1}+{k2}");
                KeyComboHook.Add((k1, k2), callback);
            } 
        }
        public static void AddMouseEvent(VirtualKeyCode key, KeyState state, System.Action<int, int> callback)
        {
            if (MouseHook.ContainsKey((key,state)))
            {
                Logger.Log("Add Mouse Hook", $"{key} {state}");
                MouseHook[(key,state)] += callback;
            }
            else
            {
                Logger.Log("Add New Mouse Hook", $"{key} {state}");
                MouseHook.Add((key, state), callback);
            } 
        }
        public static void AddKeyboardEvent(VirtualKeyCode key, KeyState state, System.Action callback)
        {
            if (KeyboardHook.ContainsKey((key, state)))
            {
                Logger.Log("Add Mouse Hook", $"{key} {state}");
                KeyboardHook[(key, state)] += callback;
            }
            else
            {
                Logger.Log("Add New Kb Hook", $"{key} {state}");
                KeyboardHook.Add((key, state), callback);
            } 
        }
        private static void InputManager_OnMouseEvent(VirtualKeyCode key, KeyState state, int x, int y)
        { 

            System.Action<int, int> callback;
            MouseHook.TryGetValue((key, state), out callback);
            if(callback == null) 
                return; 
            else 
                callback?.Invoke(x,y); 

        
        }

        private static void InputManager_OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {
            System.Action callback;
            //Console.WriteLine($"{key}, {state}");

            Console.WriteLine($"{key} {state}");
            KeyboardHook.TryGetValue((key, state), out callback);
           
            if (callback != null) 
                callback?.Invoke(); 

            if(state == KeyState.Down && _ComboStartKey == VirtualKeyCode.Invalid)
            {
                //Logger.Log("KeyCombo", $"Start Combo {key}");
                //Console.WriteLine("Start combo " + key);
                _ComboStartKey = key;
            }
            if(state == KeyState.Down && _ComboStartKey != VirtualKeyCode.Invalid)
            {
                if (_ComboStartKey != key)
                {
                    if(Logging)
                    Logger.Log("KeyCombo", $"Complate Combo {_ComboStartKey} + {key}");
                    //Console.WriteLine("Combo hitted " + $"{_ComboStartKey} + {key}");
                    var result = KeyComboHook.TryGetValue((_ComboStartKey, key), out var comboCallback);
                    if (result) 
                        comboCallback?.Invoke();  
                }
            }
            if (state == KeyState.Up && _ComboStartKey != VirtualKeyCode.Invalid && key == _ComboStartKey)
            {
                //Console.WriteLine("End combo");
                _ComboStartKey = VirtualKeyCode.Invalid;
            }
             
        }
    }
}
