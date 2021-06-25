using AutoHotInterception;
using AutoHotInterception.Helpers;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Temporary
{
    public class AHIInputController : BaseInputController
    {
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(int wCode, int wMapType);
        public int MouseID = 11;
        public int KeyboardID = 1;
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

        public override bool IsInitialized { get;  set; }
        public override void IfNeedInitialize()
        {
            var im = Im;
        }

        public ushort GetScanCode(VirtualKeyCode vk)
        {
            var scancode = MapVirtualKey((int)vk, 0);
            return (ushort)scancode;
        }
        public override void KeyDown(VirtualKeyCode keycode)
        { 
            Im.SendKeyEvent(KeyboardID, GetScanCode(keycode), 1);
            System.Threading.Thread.Sleep(5);
        }

        public override void KeyPress(VirtualKeyCode keycode)
        {
            Im.SendKeyEvent(KeyboardID, GetScanCode(keycode), 1);
            System.Threading.Thread.Sleep(5);
        }

        public override void KeyUp(VirtualKeyCode keycode)
        {
            Im.SendKeyEvent(KeyboardID, GetScanCode(keycode), 0);
            System.Threading.Thread.Sleep(5);
        }

        public override void MouseClick(VirtualKeyCode key)
        { 
            if(key == VirtualKeyCode.Lbutton)
            {
                Im.SendMouseButtonEvent(MouseID, 0, 1);
                Im.SendMouseButtonEvent(MouseID, 0, 0);
            } 
            System.Threading.Thread.Sleep(5);
        }

        public override void MouseDoubleClick(VirtualKeyCode key)
        {
            if (key == VirtualKeyCode.Lbutton)
            {
                Im.SendMouseButtonEvent(MouseID, 0, 1);
                Im.SendMouseButtonEvent(MouseID, 0, 0);
            }
            System.Threading.Thread.Sleep(5);
        }

        public override void MouseDown(VirtualKeyCode key)
        {
            if (key == VirtualKeyCode.Lbutton)
            {
                Im.SendMouseButtonEvent(MouseID, 0, 1);
            }
            System.Threading.Thread.Sleep(5);
        }

        public override void MouseDrag(VirtualKeyCode key, Vector2 start, Vector2 end)
        {
            throw new NotImplementedException();
        }

        public override void MouseUp(VirtualKeyCode key)
        {
            if (key == VirtualKeyCode.Lbutton)
            {
                Im.SendMouseButtonEvent(MouseID, 0, 0);
            }

            System.Threading.Thread.Sleep(5);
        }

        public override void MoveMouse(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void MoveMouseDirect(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
