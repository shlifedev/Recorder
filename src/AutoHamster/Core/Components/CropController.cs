using AutoHamster.Input;
using GameOverlay.Drawing;
using AutoHamster.OverlayGUI;
using LowLevelInput.Hooks;
using OpenCvSharp;
using Pixel;
using System.Threading.Tasks;
using static Pixel.Utility;
using Point = OpenCvSharp.Point;
using System;

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
        public bool Cropping { get; private set; } = false;

        private Mat _outputResult = null;
        private Point _outputCenter;
        private CreateImage _cropImage = null;
        private bool _enableDebug = true;

        private void InitializeDebug()
        {
            //계속 이미지 서칭을 하면서 테스트하기위한 쓰레드
            _ = Task.Run(() =>
            {
                while (true)
                {
                    if (_enableDebug)
                    {
                        //크랍 이미지가 존재하는 경우에만 디버그 시도
                        if (_cropImage != null)
                        {
                            var captureScreen = Utility.CaptureScreenToBitmap();
                            var result = Pixel.Compare.Match(captureScreen, _cropImage.Bitmap);
                            if (result.IsMatch() == false)
                            {
                                _outputResult = null;
                                _outputCenter = new Point(0, 0);
                                continue;
                            }
                            _outputCenter = result.Center;
                            _outputResult = result.ResultMat;
                        }
                    }

                    else
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            });

        }

        private void InitializeHookEvent()
        {
            Hook.AddKeyboardCombo(VirtualKeyCode.Lcontrol, VirtualKeyCode.F4, () => {
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
                    Logger.Log("Captrue");
                    CreateImage croppedScreeen = ImageFactory.CreateScreenCropImage(new OpenCvSharp.Point(_sX, _sY), new OpenCvSharp.Point(_distX, _distY), null);
                    CreateImage.SaveToFile(croppedScreeen); 

                    Logger.Log("Captrue Saved, Start Matching...");
                    CaptureScreenToBitmap().Match(out var oResult, out var oCenter, out var maxLoc, croppedScreeen.Bitmap);
                    Logger.Log("Captrue Match");
                    _cropImage = croppedScreeen;
                    _outputCenter = oCenter;
                    _outputResult = oResult;
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
                if (_outputResult != null)
                {
                    //  gfx.DrawBox2D(gfx.CreateSolidBrush(0, 0, 0, 100), gfx.CreateSolidBrush(0, 0, 0, 100), outputCenter.X - 20, outputCenter.Y - 20, outputCenter.X + 20, outputCenter.Y + 20, 3);
                    gfx.DrawRectangle(gf.GetBrush("red"), _outputCenter.X - _cropImage.Mat.Width * 1.25f, _outputCenter.Y - _cropImage.Mat.Height * 1.25f, _outputCenter.X + _cropImage.Mat.Width * 1.25f, _outputCenter.Y + _cropImage.Mat.Height * 1.25f, 3);
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