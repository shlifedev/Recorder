using AutoHotInterception;
using AutoHotInterception.Helpers;
using AutoHamster.Input;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoHamster.Input.Component
{
    public class AHIInputController : BaseInputController
    {

        public static AHIInputController Instance
        {
            get {
                if (instance == null)
                {
                    instance = new AHIInputController();
                }
                return instance;
            } 
        }
        private static AHIInputController instance;
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(int wCode, int wMapType);
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyEx(int wCode, int wMapType, int dwhkl);
        public int MouseID = 11;
        public int KeyboardID = 2;


        private Manager im;
        public Manager Im
        {
            get
            {
                if (im == null)
                {
                    im = new Manager();
                    bool isMouseInitialized = false;
                    var deviceList = Im.GetDeviceList();
                    if (deviceList.Length == 0)
                    {
                        IsInitialized = false;
                        Logger.Error(this, "cannot found device list");
                        im = null;
                        return im;

                    }
                    else
                    {
                        foreach (var value in deviceList)
                        { 
                            Logger.Log(im.GetMouseIdFromHandle(value.Handle).ToString());
                            if (value.IsMouse)
                            {
                                MouseID = im.GetMouseIdFromHandle(value.Handle);
                                isMouseInitialized = true;
                                Logger.Log(this, "isMouseInitialized");
                                break;
                            }
                        }
                    }


                    if (isMouseInitialized)
                    {
                        Logger.Log(this, "IsInitialized");
                        IsInitialized = true;
                    }
                }

                return im;
            }
        }



        public static void HookInitialize()
        {
        }
        private int GetBtn(VirtualKeyCode vk)
        {
            if (vk == VirtualKeyCode.Lbutton)
            {
                return 0;
            }
            if (vk == VirtualKeyCode.Rbutton)
            {
                return 1;
            }
            if (vk == VirtualKeyCode.Mbutton)
            {
                return 2;
            }
            if (vk == VirtualKeyCode.Xbutton1)
            {
                return 3;
            }
            if (vk == VirtualKeyCode.Xbutton2)
            {
                return 4;
            }
            return -1;
        }
        public override bool IsInitialized { get; set; }

        private int _x, _y;

        public AHIInputController()
        {
            Logger.Log(this, "Instantiate AHI Controller");

        }


        private VirtualKeyCode NormalVKToExVk(VirtualKeyCode vk)
        {
            if (vk == VirtualKeyCode.Control) return VirtualKeyCode.Lcontrol;
            if (vk == VirtualKeyCode.Menu) return VirtualKeyCode.Lmenu;
            if (vk == VirtualKeyCode.Shift) return VirtualKeyCode.Lshift;
            return vk;
        }
        public override void IfNeedInitialize()
        {
            var keyboardId = AHIInputController.Instance.KeyboardID;
            Im.SubscribeKeyboard(keyboardId, false, new System.Action<ushort, int>((keyId, state) =>
            {
                if (Hook.IO.GetType() == typeof(AHIInputController))
                {
                    var vkUint = AHIInputController.MapVirtualKey(keyId, 3); 
                    VirtualKeyCode vk = NormalVKToExVk((VirtualKeyCode)vkUint);
                    if(vk == VirtualKeyCode.Hotkey) 
                        Logger.Error(this, $"EX Key Read Failed keyId : ({keyId})"); 
                    KeyState ks = (state == 1) ? KeyState.Down : KeyState.Up;
                    Hook.InputManager_OnKeyboardEvent(vk, ks);
                } 
            }));

            Im.SubscribeMouseMoveAbsolute(MouseID, false, new Action<int, int>((x, y) => {
                Console.WriteLine(x);
            }));

            Im.SubscribeMouseButtons(MouseID, false, new Action<int, int>((key,state)=> {
                if (Hook.IO.GetType() == typeof(AHIInputController))
                {
                    VirtualKeyCode vk = VirtualKeyCode.Hotkey;
                    KeyState ks = KeyState.None;
                    vk = (key == 0) ? VirtualKeyCode.Lbutton :
                         (key == 1) ? VirtualKeyCode.Rbutton :
                         (key == 2) ? VirtualKeyCode.Mbutton :
                         (key == 3) ? VirtualKeyCode.Xbutton1 : VirtualKeyCode.Xbutton2;

                    ks = (state == 1) ? KeyState.Down : KeyState.Up;
                    Hook.InputManager_OnMouseEvent(vk, ks, 0, 0);
                }
            }), false);
             
            Im.SubscribeMouseMove(MouseID, false, new Action<int, int>((x, y) =>
            {
                if(Hook.IO.GetType() == typeof(AHIInputController))
                {
                    _x = x;
                    _y = y; 
                    Hook.InputManager_OnMouseEvent(VirtualKeyCode.Invalid, KeyState.None, x, y, true); 
                }  
            }));
        }

        public ushort GetScanCode(VirtualKeyCode vk)
        {
            var scancode = MapVirtualKey((int)vk, 0);
            return (ushort)scancode;
        }
        public override void KeyDown(VirtualKeyCode keycode)
        {
            Im.SendKeyEvent(KeyboardID, GetScanCode(keycode), 1);
            System.Threading.Thread.Sleep(1);
        }

        public override void KeyPress(VirtualKeyCode keycode)
        {
            KeyDown(keycode);
            KeyUp(keycode);
        }

        public override void KeyUp(VirtualKeyCode keycode)
        {
            Im.SendKeyEvent(KeyboardID, GetScanCode(keycode), 0);
            System.Threading.Thread.Sleep(1);
        }

        public override void MouseClick(VirtualKeyCode key)
        { 
            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 1);
            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 0);
            System.Threading.Thread.Sleep(5);
        }
        public override void MouseClick(VirtualKeyCode key, Vector2 position)
        {
            MoveMouseDirect((int)position.X, (int)position.Y); 
            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 1);
            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 0);
            System.Threading.Thread.Sleep(5);
        }
        public override void MouseDoubleClick(VirtualKeyCode key)
        { 
            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 1);
            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 0); 
            System.Threading.Thread.Sleep(5);
        }

        public override void MouseDown(VirtualKeyCode key)
        {
            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 1);
            System.Threading.Thread.Sleep(5);
        }

        public override void MouseDrag(VirtualKeyCode key, Vector2 start, Vector2 end)
        {
            throw new NotImplementedException();
        }

        public override void MouseUp(VirtualKeyCode key)
        {

            Im.SendMouseButtonEvent(MouseID, GetBtn(key), 0);

            System.Threading.Thread.Sleep(5);
        }

        public override void MoveMouse(int x, int y)
        {
            Im.SendMouseMoveRelative(MouseID, x, y);
        }

        public override void MoveMouseDirect(int x, int y)
        {
            int aX = (int)((ushort.MaxValue / (1920)) * x);
            int aY = (int)((ushort.MaxValue / (1080)) * y);
            im.SendMouseMoveAbsolute(MouseID, aX, aY);
        }


    }
}
