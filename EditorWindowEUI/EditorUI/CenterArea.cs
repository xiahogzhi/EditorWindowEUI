using System.Collections;
using System.Collections.Generic;
using EditorWindowEUI.Tools;
using EditorWindowEUI.UI;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.Window
{
    public class CenterArea : BaseUI
    {
        protected override void OnDrawElement()
        {
            EditorGUI.DrawRect(RenderRectInfo,EGUITools.GetColor(0.1f));
        }
    }
}