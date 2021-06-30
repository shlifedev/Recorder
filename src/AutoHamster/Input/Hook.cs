using AutoHotInterception;
using AutoHamster.Component;
using LowLevelInput.Converters;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace AutoHamster.Input
{
    public static class Hook
    {
        static IInputController io;

        public enum ControllerType
        {
            AHI = 0, User32 = 1
        }
        public struct HookMouseEvent
        {
            public ControllerType controllerType;
            public bool isMoveEventDelta;
            public bool isMoveEvent;
            /// <summary>
            /// 0 = down
            /// 1 = up 
            /// </summary>
            public int state;
            /// <summary>
            /// 0 = left
            /// 1 = right
            /// 2 = middle
            /// 3 = xButton1
            /// 4 = xButton2
            /// </summary>
            public int mouseButton;
            /// <summary>
            /// position
            /// </summary>
            public int x, y;

            public HookMouseEvent(ControllerType ctrl, bool isMoveEvent, int state, int mouseButton, int x, int y, int deltaX, int deltaY)
            {
                this.isMoveEvent = isMoveEvent;
                this.state = state;
                this.mouseButton = mouseButton;
                this.x = x;
                this.y = y;
                this.isMoveEventDelta = false;
                this.controllerType = ctrl;
            }

            public override string ToString()
            {
                return $"{state} {mouseButton} {x} {y}";
            }
        }

        public struct HookKeyEvent
        {
            public ControllerType controllerType;
            public KeyState state;
            public VirtualKeyCode vkCode;
            public HookKeyEvent(KeyState state, VirtualKeyCode vkCode)
            {
                this.state = state;
                this.vkCode = vkCode;
                this.controllerType = ControllerType.User32;
            }
            public override string ToString()
            {
                return $"{vkCode} {state}";
            }
        }

        public static System.Action<HookMouseEvent> onMouseEvent;
        public static System.Action<HookKeyEvent> onKeyboardEvent;

        public static Dictionary<(VirtualKeyCode key, KeyState state), System.Action<int, int>> MouseHook = new Dictionary<(VirtualKeyCode key, KeyState state), System.Action<int, int>>();
        public static Dictionary<(VirtualKeyCode key, KeyState state), System.Action> KeyboardHook = new Dictionary<(VirtualKeyCode key, KeyState state), Action>();
        public static Dictionary<(VirtualKeyCode k1, VirtualKeyCode k2), System.Action> KeyComboHook = new Dictionary<(VirtualKeyCode k1, VirtualKeyCode k2), Action>();
        private static bool Logging = false;
        public static int mouseX = int.MinValue, mouseY = int.MinValue;
        public static LowLevelInput.Hooks.InputManager inputManager;

        static VirtualKeyCode _ComboStartKey = VirtualKeyCode.Invalid;
        public static IInputController IO { get => io; }
        public static void IOInitialize<T>() where T : IInputController
        {
            var type = typeof(T);
            if (type == typeof(User32InputController))
                io = new User32InputController();
            else if (type == typeof(AHIInputController))
                io = AHIInputController.Instance;
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
            inputManager.OnKeyboardEvent += InputManager_OnKeyboardEvent_LowLevelInput;
            inputManager.OnMouseEvent += InputManager_OnMouseEvent_LowLevelInput;
            inputManager.Initialize();
            IOInitialize<AHIInputController>();
            IO.IfNeedInitialize();
            onInited?.Invoke();

            Hook.AddMouseEvent(VirtualKeyCode.Invalid, KeyState.None, (x, y) =>
            {
                mouseX = x;
                mouseY = y;
            });

        }


        public static void InputManager_OnKeyboardEvent_LowLevelInput(VirtualKeyCode key, KeyState state)
        {
            if (IO.GetType() == typeof(User32InputController))
            {
                InputManager_OnKeyboardEvent(key, state);
            }
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
            if (MouseHook.ContainsKey((key, state)))
            {
                Logger.Log("Add Mouse Hook", $"{key} {state}");
                MouseHook[(key, state)] += callback;
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

        public static void InputManager_OnMouseEvent_LowLevelInput(VirtualKeyCode key, KeyState state, int x, int y)
        {
            if (IO.GetType() == typeof(User32InputController))
            {
                InputManager_OnMouseEvent(key, state, x, y);
            }
        }
        public static void InputManager_OnMouseEvent(VirtualKeyCode key, KeyState state, int x, int y, bool delta = false)
        {

            System.Action<int, int> callback;
            MouseHook.TryGetValue((key, state), out callback);


            Console.WriteLine($"{key}, {state}");
            if (key == VirtualKeyCode.Invalid)
            {
                onMouseEvent?.Invoke(new HookMouseEvent()
                {
                    controllerType = ControllerType.User32,
                    isMoveEvent = true,
                    isMoveEventDelta = delta,
                    mouseButton = -1,
                    state = -1,
                    x = x,
                    y = y
                });
            }
            else
            {
                int mouseBtn = 0;
                switch (key)
                {
                    case VirtualKeyCode.Lbutton:
                        mouseBtn = 0;
                        break;

                    case VirtualKeyCode.Rbutton:
                        mouseBtn = 1;
                        break;

                    case VirtualKeyCode.Mbutton:
                        mouseBtn = 2;
                        break;

                    case VirtualKeyCode.Xbutton1:
                        mouseBtn = 3;
                        break;

                    case VirtualKeyCode.Xbutton2:
                        mouseBtn = 4;
                        break;
                }

                int _state = -1;
                if (state == KeyState.Down) _state = 0;
                if (state == KeyState.Up) _state = 1;
                if (key == VirtualKeyCode.Lbutton || key == VirtualKeyCode.Rbutton || key == VirtualKeyCode.Mbutton || key == VirtualKeyCode.Xbutton1 || key == VirtualKeyCode.Xbutton2)
                {
                    onMouseEvent?.Invoke(new HookMouseEvent()
                    {
                        controllerType = ControllerType.User32,
                        isMoveEvent = false,
                        mouseButton = mouseBtn,
                        state = _state,
                        x = x,
                        y = y
                    });
                }

            }



            if (callback == null)
                return;
            else
                callback?.Invoke(x, y);


        }


        public static void InputManager_OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {
            System.Action callback;
            Console.WriteLine($"{key}, {state}");

            onKeyboardEvent?.Invoke(new HookKeyEvent()
            {
                vkCode = key,
                state = state
            });

            KeyboardHook.TryGetValue((key, state), out callback);

            if (callback != null)
                callback?.Invoke();

            if (state == KeyState.Down && _ComboStartKey == VirtualKeyCode.Invalid)
            {
                //Logger.Log("KeyCombo", $"Start Combo {key}");
                //Console.WriteLine("Start combo " + key);
                _ComboStartKey = key;
            }
            if (state == KeyState.Down && _ComboStartKey != VirtualKeyCode.Invalid)
            {
                if (_ComboStartKey != key)
                {
                    if (Logging)
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
