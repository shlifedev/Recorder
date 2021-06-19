﻿using ByulMacro.Input;
using GameOverlay.Drawing;
using HOSAuto.Overlay;
using OpenCvSharp;
using Pixel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Point = OpenCvSharp.Point;

namespace ByulMacro.Byul.Components
{
    /// <summary>
    /// 이미지를 크랍하고, 태그를 지정하고 저장 가능한 컴퍼넌트
    /// </summary> 
    public class CropController
    {
        private int _sX, _sY, _lX, _lY, _distX, _distY;
        private bool _use = false; 

        /// <summary>
        /// 크랍 컨트롤러를 사용하는경우
        /// </summary>
        public bool Use { get => _use; set => _use = value; } 

        /// <summary>
        /// 현재 크랍을 시도중일때 true
        /// </summary>
        public bool Cropping = false;


        private Mat outputResult = null;
        private Point outputCenter;
        private CreateImage cropImage = null;
        private bool _enableDebug = true; 
        private int _debugStack = 0;  
        public void InitializeDebug()
        {

            //계속 이미지 서칭을 하면서 테스트하기위한 쓰레드
            _ = Task.Run(() =>
            {
                while (true)
                {
                    if (_enableDebug)
                    {
                        //크랍 이미지가 존재하는 경우에만 디버그 시도
                        if (cropImage != null)
                        { 
                            Pixel.Utility.CaptureScreenToBitmap().Match(out var oResult, out var oCenter, out var maxLoc, cropImage.Bitmap);
                            if (oResult == null)
                            {
                                _debugStack++;
                                //자연스럽게 사각형을 랜더링 하기위해 사용
                                if (_debugStack == 3) 
                                    outputResult = null; 
                                continue;
                            }
                          
                            outputCenter = oCenter;
                            outputResult = oResult;
                        }
                    }

                    else
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            });

        }

        public void InitializeHookEvent()
        {

            //마우스를 클릭한경우
            Hook.AddMouseEvent(LowLevelInput.Hooks.VirtualKeyCode.Rbutton, LowLevelInput.Hooks.KeyState.Down, (x, y) =>
            {
                _sX = x;
                _sY = y;
                Cropping = true;
            });

            //마우스를 Up한경우
            Hook.AddMouseEvent(LowLevelInput.Hooks.VirtualKeyCode.Rbutton, LowLevelInput.Hooks.KeyState.Up, (x, y) =>
            {
                _lX = x;
                _lY = y;
                _distX = (_lX - _sX);
                _distY = (_lY - _sY);
                Cropping = false;
                System.Threading.Thread.Sleep(500);
                CreateImage croppedScreeen = ImageFactory.CreateScreenCropImage(new OpenCvSharp.Point(_sX, _sY), new OpenCvSharp.Point(_distX, _distY), null);
                //현재 스크린에 크랍 이미지 체크   
                Pixel.Utility.CaptureScreenToBitmap().Match(out var oResult, out var oCenter, out var maxLoc, croppedScreeen.Bitmap);
                cropImage = croppedScreeen;
                outputCenter = oCenter;
                outputResult = oResult;
            });
            //마우스를 움직이는 도중
            Hook.AddMouseEvent(LowLevelInput.Hooks.VirtualKeyCode.Invalid, LowLevelInput.Hooks.KeyState.None, (x, y) =>
            {
                if (Cropping)
                {
                    _lX = x;
                    _lY = y;
                    _distX = (_lX - _sX);
                    _distY = (_lY - _sY);
                }
            });
        }

        public void Render(GraphicsFactory gf, Graphics gfx) { 
            gfx.ClearScene();
            if (outputResult != null)
            {
                gfx.DrawBox2D(gfx.CreateSolidBrush(0, 0, 0, 100), gfx.CreateSolidBrush(0, 0, 0, 100), outputCenter.X - 20, outputCenter.Y - 20, outputCenter.X + 20, outputCenter.Y + 20, 3);
                gfx.DrawRectangle(gf.GetBrush("red"), outputCenter.X - 20, outputCenter.Y - 20, outputCenter.X + 20, outputCenter.Y + 20, 3);
            }
            if (Cropping)
            {
                gfx.DrawBox2D(gfx.CreateSolidBrush(0, 0, 0, 100), gfx.CreateSolidBrush(0, 0, 0, 100), _sX, _sY, _lX, _lY, 3);
                gfx.DrawRectangle(gf.GetBrush("green"), _sX, _sY, _lX, _lY, 3);
            }
        }
        public CropController()
        { 
            InitializeDebug();
            InitializeHookEvent();
            new Renderer(1920, 1080, Render).Run();  
        }
    }
}