using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.Tools
{
    public class EGUITools
    {
        public static Color GetColor(float color)
        {
            if (EditorGUIUtility.isProSkin)
            {
                return new Color(color, color, color, 1);
            }

            float c = 1 - color;
            return new Color(c, c, c, 1);
        }
    }
}