﻿using Coroutine;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using Pixel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ByulMacro.Input;
using HOSAuto.Overlay;
using ByulMacro.Byul.Components;

namespace ByulMacro
{
    public partial class MainWindow : System.Windows.Window
    {
        /// <summary>
        /// 콘솔 할당용
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();


        private Renderer externalOverlay = null;
        private CropController cropController;
        private GUI.Overlay overlay = new GUI.Overlay();
        private DateTime lastTime;



        public void InitializeExOverlayGUI()
        {
            externalOverlay  = new Renderer(1920, 1080, (gf, gfx)=> {
                gfx.ClearScene(); 
                gfx.DrawText(gf.GetFont("arial_big"), gf.GetBrush("white"), new GameOverlay.Drawing.Point(5, 2), $"FPS : {gfx.FPS}");
            });
            externalOverlay.Run();
        } 
        /// <summary>
        /// 새로운 쓰래드에 코루틴용 틱 계산기 할당 
        /// </summary>
        public void InitializeCoroutine()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var currTime = DateTime.Now;
                    CoroutineHandler.Tick(currTime - lastTime);
                    lastTime = currTime;
                    Thread.Sleep(1);
                }
            });
            CoroutineHandler.Start(WaitSeconds());
        } 
        public MainWindow()
        {
            AllocConsole(); // 콘솔 할당
            InitializeComponent(); //component 초기화
            InitializeExOverlayGUI(); // gui 초기화  
            InitializeHookEvent();
            // InitializeCoroutine(); 

            Hook.HookInit(); // 입력 이벤트 추가

            cropController = new CropController();
        }

        //이미지 크랍
        private int startX, startY, lastX, lastY, distX, distY; 
        private void InitializeHookEvent()
        { 
            
        }
        private static IEnumerator<Wait> WaitSeconds()
        {
            yield return new Wait(1);
            Console.WriteLine("First thing " + DateTime.Now);
            yield return new Wait(1);
            Console.WriteLine("After 1 second " + DateTime.Now);

            yield return new Wait(1);
            Console.WriteLine("After 2 second " + DateTime.Now);

            yield return new Wait(1);
            Console.WriteLine("After 3 second " + DateTime.Now);
            yield return new Wait(5);
            Console.WriteLine("After 5 seconds " + DateTime.Now);
            yield return new Wait(10);
            Console.WriteLine("After 10 seconds " + DateTime.Now);
        }


        /// <summary>
        /// 실험용 테스트코드 작성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateImage test = ImageFactory.CreateScreenCropImage(new OpenCvSharp.Point(300, 410), new OpenCvSharp.Point(150, 40), "test");
            test.Mat.SaveImage("test/test.png");
            Pixel.Utility.CaptureScreenToBitmap().Match(out var result, out var center, out var maxLoc, test.Bitmap);

            Cv2.ImShow("result", result);
        }
    }
}
