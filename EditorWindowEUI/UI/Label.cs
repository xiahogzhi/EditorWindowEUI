using UnityEngine;

namespace EditorWindowEUI.UI
{
    public class Label : BaseUI
    {
        public string Text { set; get; }


        protected override void OnDrawElement()
        {
            GUI.skin.label.Draw(RenderRectInfo, Text, false, false, false, false);
        }
    }
}