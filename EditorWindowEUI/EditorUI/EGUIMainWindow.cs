using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.Window
{
    public class EGUIMainWindow : BaseEditorWindowGUI
    {
        [MenuItem("Tools/EGUI Window")]
        public static void Open()
        {
            EGUIMainWindow ew = GetWindow<EGUIMainWindow>(false, "EGUI Window");
            ew.Show();
        }


        protected override void OnInitialize()
        {
        }

        protected override void OnLayoutBuild()
        {
            ToolsLeftArea tools = EuiCore.CreateElement<ToolsLeftArea>();
            tools.SetAnchor(AnchorType.LeftStretch);
            tools.Pivot = new Vector2(0, 1);
            tools.Width = 150;

            CenterArea ca = EuiCore.CreateElement<CenterArea>();
            ca.SetAnchor(AnchorType.Stretch);
            ca.OffsetMin = new Vector2(150, 0);
            
        }
    }
}