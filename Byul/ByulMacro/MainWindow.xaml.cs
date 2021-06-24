using Coroutine;
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
using ByulMacro.GUI;
using ByulMacro.Byul.Core;
using ByulMacro.Byul.Attributes;
using System.Diagnostics;
using System.Windows.Threading;
using LowLevelInput.Hooks;

namespace ByulMacro
{
    public partial class MainWindow : System.Windows.Window
    {

        public Process targetProcess = null;
        /// <summary>
        /// 콘솔 할당용
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();


        private Renderer externalOverlay = null;
        private CropController cropController; 
        private DateTime lastTime;
      
       
        public void InitializeExOverlayGUI()
        {
           
            externalOverlay  = new Renderer(1920, 1080, (gf, gfx)=> {
                gfx.ClearScene(); 
                gfx.DrawText(gf.GetFont("arial_big"), gf.GetBrush("white"), new GameOverlay.Drawing.Point(5, 2), $"FPS : {gfx.FPS}");
                 
            });
            externalOverlay.Run();
        }
         
        [Byul.Attributes.Run]
        public static void HookByulExit()
        {
             
            Hook.AddKeyboardCombo(VirtualKeyCode.Lcontrol, VirtualKeyCode.One, () => {
                Console.WriteLine("Test Exit" + Thread.CurrentThread.ManagedThreadId); 
                Dispatcher.CurrentDispatcher.Invoke(() => {
                    Process.GetCurrentProcess().Kill();
                }); 
            }); 

        }

        [Byul.Attributes.Run]
        public static void InitGUIEventHandler() => GUIEventHandler.OnSelectProcess += (process) =>
        { 
            var mw = (ByulMacro.MainWindow)Application.Current.MainWindow;
            mw.targetProcess = process; 
        };


        public MainWindow()
        {
            AllocConsole(); // 콘솔 할당 
            InitializeComponent(); //component 초기화
            InitializeExOverlayGUI(); // gui 초기화   
            Hook.HookInit(); // 입력 이벤트 추가

          
            Overlay.Run();  
           
            cropController = new CropController();
          
            RuninTaskInitializer.Init();


            var students = new List<Byul.Core.Command>() {
                new CommandImageFindAndClick()
            }; 
            V_ContentList.ItemsSource = students; 
        } 
    }
}
