using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorWindowEUI
{
    public class Button : BaseUI
    {
        public event Action OnClickEvt;

        protected override GUIStyle GetStyle()
        {
            return GUI.skin.button;
        }


        public override void OnClick(Vector2 pos)
        {
            base.OnClick(pos);
            OnClickEvt?.Invoke();
        }
    }
}