using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace EditorWindowEUI.UI
{
    public class Button : BaseUI
    {
        public event Action OnClickEvt;

        private GUIStyle _style;

        public GUIStyle Style
        {
            get => _style;
            set => _style = value;
        }


        protected override void OnDrawElement()
        {
            if (Style != null)
            {
                Style.Draw(RenderRectInfo, Text, IsHover, IsPress, false, false);
            }
            else
            {
                GUI.skin.button.Draw(RenderRectInfo, Text, IsHover, IsPress, false, false);
            }
        }

        public override void OnClick(Vector2 pos)
        {
            base.OnClick(pos);
            OnClickEvt?.Invoke();
        }
    }
}