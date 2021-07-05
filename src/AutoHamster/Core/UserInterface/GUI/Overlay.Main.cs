using AutoHamster.GUI.Command;
using AutoHamster.Input;
using Coroutine;
using LowLevelInput.Hooks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoHamster.GUI
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