using UnityEngine;

namespace EditorWindowEUI
{
    public class Label : BaseUI
    {
        public string Text { set; get; }


        protected override GUIStyle GetStyle()
        {
            return GUI.skin.label;
        }

        protected override void OnUIDraw()
        {
            GetStyle().Draw(RenderInfo, Text, false, false, false, false);
        }
    }
}