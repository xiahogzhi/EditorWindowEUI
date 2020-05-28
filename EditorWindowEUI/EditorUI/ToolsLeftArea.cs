using System;
using System.Collections;
using System.Collections.Generic;
using EditorWindowEUI.EditorUI;
using EditorWindowEUI.Tools;
using EditorWindowEUI.UI;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.Window
{
    public class ToolsLeftArea : BaseUI
    {
        private List<Type> _types = new List<Type>();

        private Dictionary<Type, Type> _bindUis = new Dictionary<Type, Type>();

        public override void OnLayerBuild()
        {
            base.OnLayerBuild();
            StackLayout sl = CurEuiCore.CreateElement<StackLayout>();
            sl.SetParent(this);
            sl.SetAnchor(AnchorType.Stretch);

            Type[] types = typeof(ToolsLeftArea).Assembly.GetTypes();
            foreach (var type in types)
            {
                BindUI bingUI = type.GetCustomAttribute<BindUI>();
                if (bingUI != null && type.IsSubclassOf(typeof(BaseEditorUI)) && !_bindUis.ContainsKey(bingUI.Ui))
                {
                    _types.Add(type);
                    Button btn = CurEuiCore.CreateElement<Button>();
                    btn.SetParent(sl);
                    btn.Height = 30;
                    btn.Text = bingUI.Ui.Name;
                }
            }
        }

        protected override void OnDrawElement()
        {
            EditorGUI.DrawRect(RenderRectInfo, EGUITools.GetColor(0.15f));
        }
    }
}