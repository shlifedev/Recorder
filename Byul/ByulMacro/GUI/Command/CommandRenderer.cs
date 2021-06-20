﻿using ImGuiNET;

namespace ByulMacro.GUI.Command
{
 
    public class CommandRenderer : ICommandRenderer
    {
        /// <summary>
        /// 리스트의 순서대로 인덱스가 지정됨. 0번배열의 경우 0번, 3번 배열의경우 3번
        /// </summary>
        public int index = 0; 
        public virtual void Render()
        {
            //테스트 랜더링 소스
            ImGui.Text($"[{index}]");
            ImGui.SameLine();
            if (ImGui.TreeNode($"[{index}] Find Image And Click Other"))
            {
                ImGui.Button("crop");
                ImGui.TreePop();
            }
        }
    }
}