using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.UI
{
    public class VerticalScrollRect : BaseUI, IKeyDownHandler
    {
        private VerticalScrollbar _verticalScrollbar;

        private LayoutUIElement _content;

        public void SetContent(LayoutUIElement content)
        {
            _content = content;
            _content.SetParent(this);
            _content.SetAnchor(AnchorType.TopStretch);
            _content.Pivot = new Vector2(0.5f, 0);
            _content.Size = new Vector2(100, 200);
            _content.SetIndex(0);
            RefreshContentPosition();
        }

        public void AddContent(LayoutUIElement ui)
        {
            if (_content == null)
                return;

            ui.SetParent(_content);
        }


        public void SetContentY(float y)
        {
            if (_content == null)
                return;

            if (y < 0)
            {
                y = 0;
            }
            else if (y > _content.RectInfo.height - RectInfo.height)
            {
                y = _content.RectInfo.height - RectInfo.height;
            }

//            _content.AnchordPosition = new Vector2(0, y);
            _verticalScrollbar.SetValue(y / (_content.RectInfo.height - RectInfo.height));
        }

        public override void OnLayerBuild()
        {
            base.OnLayerBuild();
            _verticalScrollbar = CurEuiCore.CreateElement<VerticalScrollbar>();
            _verticalScrollbar.SetAnchor(AnchorType.RightStretch);
            _verticalScrollbar.Pivot = new Vector2(1, 0.5f);
            _verticalScrollbar.SetParent(this);
            _verticalScrollbar.OnValueChanged += (x) => { RefreshContentPosition(); };
            _verticalScrollbar.SetSliderHeightRelative(0.4f);
            SetActiveClip(true);
        }


        void RefreshContentPosition()
        {
            if (_content == null)
                return;

            _content.AnchoredPosition =
                new Vector2(0, -_verticalScrollbar.Value * (_content.RectInfo.height - RectInfo.height));
        }


        protected override void OnDrawElement()
        {
            if (_content != null)
            {
                float v = RectInfo.height / _content.RectInfo.height;
                _content.OffsetMax = new Vector2(13, 0);
                _verticalScrollbar.SetSliderHeightRelative(v);
            }

            _verticalScrollbar.ScrollValue(Event.current.delta.y);

            DrawStyle(GUI.skin.box);
//            EditorGUI.DrawRect(RenderRectInfo, Color.cyan);
        }

        public void OnKeyDown(KeyCode k, bool isShift, bool isCtrl, bool isAlt)
        {
            if (k == KeyCode.KeypadMinus)
            {
                Size -= new Vector2(0, 10);
            }

            if (k == KeyCode.KeypadPlus)
            {
                Size += new Vector2(0, 10);
            }
        }
    }
}