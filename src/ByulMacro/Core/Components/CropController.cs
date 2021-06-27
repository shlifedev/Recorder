using AutoHamster.Input;
using GameOverlay.Drawing;
using AutoHamster.OverlayGUI;
using LowLevelInput.Hooks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Pixel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Pixel.Utility;
using Point = OpenCvSharp.Point;

namespace AutoHamster.Core.Components
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
        public bool Use
        {
            get => _use; set
            {
                this._use = value;
                Logger.Log("Crop Controller State", value.ToString());
            }
        }

        /// <summary>
        /// 현재 크랍을 시도중일때 true
        /// </summary>
        public bool Cropping = false;


        private Mat outputResult = null;
        private Point outputCenter;
        private CreateImage cropImage = null;
        private bool _enableDebug = true;
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
                            var captureScreen = Utility.CaptureScreenToBitmap();
                            var result = Pixel.Compare.Match(captureScreen, cropImage.Bitmap);
                            if (result.IsMatch() == false)
                            {
                                outputResult = null;
                                outputCenter = new Point(0, 0);
                                continue;
                            }
                            outputCenter = result.center;
                            outputResult = result.resultMat;
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
            Hook.AddKeyboardCombo(VirtualKeyCode.Lcontrol, VirtualKeyCode.Four, () => {
                Use = !Use;
            });

            //마우스를 클릭한경우
            Hook.AddMouseEvent(VirtualKeyCode.Rbutton, KeyState.Down, (x, y) =>
            {
                if (Use)
                {
                    _sX = x;
                    _sY = y;
                    Cropping = true;
                }
            });

            //마우스를 Up한경우
            Hook.AddMouseEvent(VirtualKeyCode.Rbutton, KeyState.Up, (x, y) =>
            {
                if (Use)
                {
                    _lX = x;
                    _lY = y;
                    _distX = (_lX - _sX);
                    _distY = (_lY - _sY);
                    Cropping = false;
                    System.Threading.Thread.Sleep(500);
                    CreateImage croppedScreeen = ImageFactory.CreateScreenCropImage(new OpenCvSharp.Point(_sX, _sY), new OpenCvSharp.Point(_distX, _distY), null);
                    CaptureScreenToBitmap().Match(out var oResult, out var oCenter, out var maxLoc, croppedScreeen.Bitmap);
                    cropImage = croppedScreeen;
                    outputCenter = oCenter;
                    outputResult = oResult;
                }
            });
            //마우스를 움직이는 도중
            Hook.AddMouseEvent(LowLevelInput.Hooks.VirtualKeyCode.Invalid, LowLevelInput.Hooks.KeyState.None, (x, y) =>
            {
                if (Cropping && Use)
                {
                    _lX = x;
                    _lY = y;
                    _distX = (_lX - _sX);
                    _distY = (_lY - _sY);
                }
            });
        }

        public void Render(GraphicsFactory gf, Graphics gfx)
        {
            gfx.ClearScene();
            if (Use)
            {
                if (outputResult != null)
                {
                    //  gfx.DrawBox2D(gfx.CreateSolidBrush(0, 0, 0, 100), gfx.CreateSolidBrush(0, 0, 0, 100), outputCenter.X - 20, outputCenter.Y - 20, outputCenter.X + 20, outputCenter.Y + 20, 3);
                    gfx.DrawRectangle(gf.GetBrush("red"), outputCenter.X - cropImage.Mat.Width * 1.25f, outputCenter.Y - cropImage.Mat.Height * 1.25f, outputCenter.X + cropImage.Mat.Width * 1.25f, outputCenter.Y + cropImage.Mat.Height * 1.25f, 3);
                }
                if (Cropping)
                {
                    gfx.DrawBox2D(gfx.CreateSolidBrush(0, 0, 0, 235), gfx.CreateSolidBrush(0, 0, 0, 100), _sX, _sY, _lX, _lY, 3);
                    gfx.DrawRectangle(gf.GetBrush("green"), _sX, _sY, _lX, _lY, 6);
                }
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