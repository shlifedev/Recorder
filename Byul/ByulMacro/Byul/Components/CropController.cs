using ByulMacro.Input;
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
        private int startX, startY, lastX, lastY, distX, distY;
        private bool use = false;
        public Renderer cropRenderer;

        public bool Use { get => use; set => use = value; }

        public bool cropSwitch = false;

        public Mat result = null;
        public Point dbgCenter;
        public CreateImage latestCroppedImg = null;
        public CropController()
        {
            Console.WriteLine("Initialize Crop Controller");


            _ = Task.Run(() =>
            { 
                while (true)
                { 
                    if (latestCroppedImg != null)
                    {
                        Pixel.Utility.CaptureScreenToBitmap().Match(out var oResult, out var oCenter, out var maxLoc, latestCroppedImg.Bitmap);
                        dbgCenter = oCenter;
                        result = oResult; 
                    }
                }
            });

            Hook.AddMouseEvent(LowLevelInput.Hooks.VirtualKeyCode.Rbutton, LowLevelInput.Hooks.KeyState.Down, (x, y) =>
            {
                startX = x;
                startY = y;
                cropSwitch = true;
            });
            Hook.AddMouseEvent(LowLevelInput.Hooks.VirtualKeyCode.Rbutton, LowLevelInput.Hooks.KeyState.Up, (x, y) =>
            {
                lastX = x;
                lastY = y;
                distX = (lastX - startX);
                distY = (lastY - startY);

                //이미지 크랍
                CreateImage croppedScreeen = ImageFactory.CreateScreenCropImage(new OpenCvSharp.Point(startX, startY), new OpenCvSharp.Point(distX, distY), null);
                latestCroppedImg = croppedScreeen;
                //현재 스크린에 크랍 이미지 체크
                Pixel.Utility.CaptureScreenToBitmap().Match(out var oResult, out var oCenter, out var maxLoc, croppedScreeen.Bitmap);
              
                
                dbgCenter = oCenter;
                result = oResult;
                cropSwitch = false;
            });
            //랜더링 초기화

            Hook.AddMouseEvent(LowLevelInput.Hooks.VirtualKeyCode.Invalid, LowLevelInput.Hooks.KeyState.None, (x, y) =>
            {
                if (cropSwitch)
                {
                    lastX = x;
                    lastY = y;
                    distX = (lastX - startX);
                    distY = (lastY - startY);
                }
            });
            //랜더
            cropRenderer = new Renderer(1920, 1080, (gf, gfx) =>
            {
                gfx.ClearScene();

                if (result != null)
                {
                    gfx.DrawBox2D(gfx.CreateSolidBrush(0, 0, 0, 100), gfx.CreateSolidBrush(0, 0, 0, 100), dbgCenter.X - 20, dbgCenter.Y - 20, dbgCenter.X + 20, dbgCenter.Y + 20, 3);
                    gfx.DrawRectangle(gf.GetBrush("red"), dbgCenter.X - 20, dbgCenter.Y - 20, dbgCenter.X + 20, dbgCenter.Y + 20, 3);

                }
                if (cropSwitch)
                {
                    gfx.DrawBox2D(gfx.CreateSolidBrush(0, 0, 0, 100), gfx.CreateSolidBrush(0, 0, 0, 100), startX, startY, lastX, lastY, 3);
                    gfx.DrawRectangle(gf.GetBrush("green"), startX, startY, lastX, lastY, 3);
                }
            });
            cropRenderer.Run();


   
        }
    }
}