using ByulMacro.Core;
using ByulMacro.Core.Attributes;
using ByulMacro.Core.Components;
using ByulMacro.GUI;
using ByulMacro.Input;
using HOSAuto.Overlay;
using LowLevelInput.Hooks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
namespace ByulMacro
{
    public partial class MainWindow : System.Windows.Window
    { 
        public Process targetProcess = null; 
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole(); 
        private Renderer externalOverlay = null;
        private CropController cropController; 

        public void InitializeExOverlayGUI()
        {
           
            externalOverlay  = new Renderer(1920, 1080, (gf, gfx)=> {
                gfx.ClearScene(); 
                gfx.DrawText(gf.GetFont("arial_big"), gf.GetBrush("white"), new GameOverlay.Drawing.Point(5, 2), $"FPS : {gfx.FPS}");
            });
            externalOverlay.Run();
        }
         
        [Run]
        public static void HookByulExit()
        {
             
            Hook.AddKeyboardCombo(VirtualKeyCode.Lcontrol, VirtualKeyCode.One, () => {
                //Console.WriteLine("Test Exit" + Thread.CurrentThread.ManagedThreadId); 
                Dispatcher.CurrentDispatcher.Invoke(() => {
                    Process.GetCurrentProcess().Kill();
                }); 
            }); 

        }

        [Run]
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
            Overlay.Instance.Run();   
            cropController = new CropController(); 
            RunAttributeInitializer.Init();


            var students = new List<Command>() {
                new CommandImageFindAndClick()
            }; 
            V_ContentList.ItemsSource = students; 
        } 
    }
}
