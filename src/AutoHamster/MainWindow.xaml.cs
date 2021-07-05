using AutoHamster.Core;
using AutoHamster.Core.Attributes;
using AutoHamster.Core.Components;
using AutoHamster.GUI;
using AutoHamster.Input;
using AutoHamster.OverlayGUI;
using LowLevelInput.Hooks;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace AutoHamster
{
    public partial class MainWindow : System.Windows.Window
    { 
        public Process TargetProcess = null; 
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole(); 
        private Renderer externalOverlay = null;
        private CropController _cropController; 
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
            Hook.AddKeyboardCombo(VirtualKeyCode.Lcontrol, VirtualKeyCode.F1, () => { 
                Dispatcher.CurrentDispatcher.Invoke(() => {
                    Process.GetCurrentProcess().Kill();
                }); 
            }); 
        }

        [Run]
        public static void InitGUIEventHandler() => GUIEventHandler.OnSelectProcess += (process) =>
        { 
            var mw = (AutoHamster.MainWindow)Application.Current.MainWindow;
            mw.TargetProcess = process; 
        };


 
         

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
                _cropController = new CropController();
                Logger.Log(this, "crop controller initialized");  
                RunAttributeInitializer.Init();  
            });

            
        } 
    }
}
