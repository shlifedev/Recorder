using ClickableTransparentOverlay;
using Coroutine;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByulMacro.GUI
{
    public partial class Overlay
    {
        public static void Run()
        {
            Task.Run(() => {
                CoroutineHandler.Start(MainLogic());

                ClickableTransparentOverlay.Overlay.RunInfiniteLoop();
            }); 
        }

        private static IEnumerator<Wait> MainLogic()
        {
            while (true)
            {
                yield return new Wait(ClickableTransparentOverlay.Overlay.OnRender);
                ImGui.Begin("이벤트 관리자");
                ImGui.ShowStyleEditor();
                ImGui.SetWindowSize(new Vector2(300, 400));
                if (ImGui.BeginMenu("이미지 찾기"))
                {
                    ImGui.Button("test 1");
                    ImGui.Text("test 2");
                    ImGui.Text("test 3");
                    ImGui.Text("test 4");
                    if (ImGui.BeginMenu("찾은 경우")) {
                        ImGui.Text("test 1");
                        ImGui.Text("test 2");
                        ImGui.Text("test 3");
                        ImGui.Text("test 4");
                    }
                        
                        
                    ImGui.EndMenu();
                }
                ImGui.EndMenu();  
                ImGui.End();
            }
        }
    }
}