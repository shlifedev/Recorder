using ClickableTransparentOverlay;
using Coroutine;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.GUI
{
    public partial class Overlay
    { 
        public void Run()
        {
            CoroutineHandler.Start(Logic()); 
            ClickableTransparentOverlay.Overlay.RunInfiniteLoop();
        }


        public IEnumerator<Wait> Logic()
        {
            while (true)
            {
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender); 
                ImGui.Begin("Heros Of Storm Auto", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize);
                ImGui.Text("안녕 내이르믕 코난"); 
                ImGui.End(); 
            }

        } 
    }

}