using ByulMacro.GUI.Command;
using ByulMacro.Input;
using ClickableTransparentOverlay;
using Coroutine;
using ImGuiNET;
using LowLevelInput.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByulMacro.GUI
{
    public partial class Overlay
    {
        public static Overlay Instance
        {
            get
            {
                if (inst == null)
                {
                    inst = new Overlay();
                    return inst;
                }
                return inst;
            }
        }
        private static Overlay inst;
        private bool showMainMenu = true;
        public List<ICommandRenderer> test = new List<ICommandRenderer>();
  
        public void Run(System.Action callback = null)
        { 
            Task.Run(() =>
            { 
                CoroutineHandler.Start(RenderMainOverlay()); 
                CoroutineHandler.Start(OverlayDebugLogger.Instance.RenderDebugger());
                CoroutineHandler.Start(OverlayProcessSelector.Instance.RenderProcessSelector());

               
                MainLogicInputHook(); 
                ClickableTransparentOverlay.Overlay.RunInfiniteLoop(); 
                callback?.Invoke();
            }); 
        }

        public void MainLogicInputHook()
        {
            Hook.KeyboardHook.Add((VirtualKeyCode.Insert, KeyState.Up), () =>
            {
                showMainMenu = !showMainMenu;
            });
        }

        private IEnumerator<Wait> RenderMainOverlay()
        {
            while (true)
            {
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender); 
            }
        }
    }
}