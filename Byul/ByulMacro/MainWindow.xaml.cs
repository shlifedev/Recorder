using AutoHotInterception;
using ByulMacro.Core;
using ByulMacro.Core.Attributes;
using ByulMacro.Core.Components;
using ByulMacro.GUI;
using ByulMacro.Input;
using HOSAuto.Overlay;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
            Logger.Log(this, ".net overlay initialized");
        }
         
        [Run]
        public static void HookForDebug()
        { 
            Hook.AddKeyboardCombo(VirtualKeyCode.Lcontrol, VirtualKeyCode.One, () => { 
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



        [RunInTask]
        public static void T()
        {

            Console.WriteLine("ZZ");
            System.Threading.Thread.Sleep(2000); 
            Hook.IO.KeyDown(VirtualKeyCode.Lcontrol);  
            Hook.IO.KeyDown(VirtualKeyCode.Two); 
            Hook.IO.KeyUp(VirtualKeyCode.Two); 
        }


        public MainWindow()
        {
            AllocConsole(); // 콘솔 할당 
            InitializeComponent(); //component 초기화 
            OverlayDebugLogger.Init();
            Overlay.Instance.Run(()=> {
                Logger.Log(this, "imgui overlay initialized");
            }); 
            System.Threading.Thread.Sleep(250); // wait for coroutine 
            Hook.HookInit(() => {

                Logger.Log(this, "io hooker initialized");
                InitializeExOverlayGUI();
                cropController = new CropController();
                Logger.Log(this, "crop controller initialized");  
                RunAttributeInitializer.Init();

 

            });

 







            var students = new List<Command>() {
                new CommandImageFindAndClick()
            };

            Task.Run(() => { 
                System.Threading.Thread.Sleep(1000); 
            });
            V_ContentList.ItemsSource = students;  
        } 
    }
}
