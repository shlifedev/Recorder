using System;
using System.Runtime.InteropServices;
 
    public static class InputSimulator
    { 
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetMessageExtraInfo();

        [Flags]
        private enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        } 

        [Flags]
        private enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008,
        }

        [Flags]
        private enum MouseDataF
        {
            None = 0,
            XButton1 = 0x0001,
            XButton2 = 0x0002
        }

        [Flags]
        private enum MouseEventF
        {
            None = 0,
            Absolute = 0x8000,
            HWheel = 0x1000,
            Move = 0x0001,
            MoveNoCoalesce = 0x2000,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            XDown = 0x0080,
            XUp = 0x0100
        }

        private struct Input
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public readonly HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public readonly int dx;
            public readonly int dy;
            public uint mouseData;
            public uint dwFlags;
            public readonly uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public readonly uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public readonly uint uMsg;
            public readonly ushort wParamL;
            public readonly ushort wParamH;
        }

        public static class Keyboard
        {
            public static void KeyDown(ushort keycode)
            {
                Input[] inputs =
                {
                            new Input
                            {
                                type = (int) InputType.Keyboard,
                                u = new InputUnion
                                {
                                    ki = new KeyboardInput
                                    {
                                        wVk = 0,
                                        wScan = keycode,
                                        dwFlags = (uint) (KeyEventF.KeyDown | KeyEventF.Scancode),
                                        dwExtraInfo = GetMessageExtraInfo()
                                    }
                                }
                            }
                        };

                SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            }

            public static void KeyUp(ushort keycode)
            {
                Input[] inputs =
                {
                            new Input
                            {
                                type = (int) InputType.Keyboard,
                                u = new InputUnion
                                {
                                    ki = new KeyboardInput
                                    {
                                        wVk = 0,
                                        wScan = keycode,
                                        dwFlags = (uint) (KeyEventF.KeyUp | KeyEventF.Scancode),
                                        dwExtraInfo = GetMessageExtraInfo()
                                    }
                                }
                            }
                        };

                SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            }

            public static void KeyPress(ushort keycode)
            {
                KeyDown(keycode);
                KeyUp(keycode);
            }
        }

    public static class Mouse
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern int SetCursorPos(int x, int y);

        /// <summary>
        /// 현재 위치로부터 xDelta, yDelta만큼 마우스 커서를 이동
        /// </summary>
        /// <param name="xDelta"></param>
        /// <param name="yDelta"></param>
        public static void Move(int xDelta, int yDelta)
        {
            mouse_event(0x0001, xDelta, yDelta, 0, 0);
        }

        public static void MoveDirect(int xDelta, int yDelta)
        {
            SetCursorPos(xDelta, yDelta);
        }

        public enum Buttons
            {
                Left,
                Right,
                Middle,
                X1,
                X2
            }

            public static void MouseDown(Buttons button)
            {

                MouseEventF flags = MouseEventF.None;
                MouseDataF data = MouseDataF.None;

                switch (button)
                {
                    case Buttons.Left:
                        flags = MouseEventF.LeftDown;
                        data = MouseDataF.None;
                        break;
                    case Buttons.Right:
                        flags = MouseEventF.RightDown;
                        data = MouseDataF.None;
                        break;
                    case Buttons.Middle:
                        flags = MouseEventF.MiddleDown;
                        data = MouseDataF.None;
                        break;
                    case Buttons.X1:
                        flags = MouseEventF.XDown;
                        data = MouseDataF.XButton1;
                        break;
                    case Buttons.X2:
                        flags = MouseEventF.XDown;
                        data = MouseDataF.XButton2;
                        break;
                }

                Input[] inputs =
                {
                            new Input
                            {
                                type = (int) InputType.Mouse,
                                u = new InputUnion
                                {
                                    mi = new MouseInput
                                    {
                                        mouseData = (uint)data,
                                        dwFlags = (uint)flags,
                                        dwExtraInfo = GetMessageExtraInfo()
                                    }
                                }
                            }
                        };

                SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            }

            public static void MouseUp(Buttons button)
            {

                MouseEventF flags = MouseEventF.None;
                MouseDataF data = MouseDataF.None;

                switch (button)
                {
                    case Buttons.Left:
                        flags = MouseEventF.LeftUp;
                        data = MouseDataF.None;
                        break;
                    case Buttons.Right:
                        flags = MouseEventF.RightUp;
                        data = MouseDataF.None;
                        break;
                    case Buttons.Middle:
                        flags = MouseEventF.MiddleUp;
                        data = MouseDataF.None;
                        break;
                    case Buttons.X1:
                        flags = MouseEventF.XUp;
                        data = MouseDataF.XButton1;
                        break;
                    case Buttons.X2:
                        flags = MouseEventF.XUp;
                        data = MouseDataF.XButton2;
                        break;
                }

                Input[] inputs =
                {
                            new Input
                            {
                                type = (int) InputType.Mouse,
                                u = new InputUnion
                                {
                                    mi = new MouseInput
                                    {
                                        mouseData = (uint)data,
                                        dwFlags = (uint)flags,
                                        dwExtraInfo = GetMessageExtraInfo()
                                    }
                                }
                            }
                        };

                SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            }

            public static void MouseClick(Buttons button)
            {
                MouseDown(button);
                MouseUp(button);
            }

            public static void MouseDoubleClick(Buttons button)
            {
                MouseClick(button);
                MouseClick(button);
            }
        }
    } 