using UnityEngine;

namespace EditorWindowEUI.UI
{
    public class StackLayout : BaseUI
    {
        protected override void OnDrawElement()
        {
            float top = 0;
            for (int i = 0; i < ChildCount; i++)
            {
                LayoutUIElement ui = GetChild(i) as LayoutUIElement;
                ui.SetAnchor(AnchorType.TopStretch);
                ui.Pivot = new Vector2(0.5f, 0);
                ui.AnchoredPosition = new Vector2(0, top);
                top += ui.RectInfo.height;
            }

            Size = new Vector2(Size.x, top);
            Rect r;
            if (Parent != null)
            {
                RectUIElement rt = Parent as RectUIElement;
                r = rt.RectInfo;
            }
            else
            {
                r = CurEuiCore.CurEditorWindow.position;
            }

            OffsetMax = new Vector2(OffsetMax.x, r.height - top);
        }
    }
}