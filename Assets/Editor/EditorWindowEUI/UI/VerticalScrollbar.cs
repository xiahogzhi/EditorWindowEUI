using System;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.UI
{
    public class VerticalScrollbar : BaseUI
    {
        protected GUIStyle VerticalSliderStyle => GUI.skin.verticalScrollbar;
        protected GUIStyle VertticalButtonUpStyle => GUI.skin.verticalScrollbarUpButton;
        protected GUIStyle VerticalButtonDownStyle => GUI.skin.verticalScrollbarDownButton;

        //当前滑动到的值
        private float _curentValue;

        public float Value
        {
            private set
            {
                if (_curentValue != value)
                {
                    _curentValue = value;
                    OnValueChanged?.Invoke(value);
                }
            }
            get { return _curentValue; }
        }

        public event Action<float> OnValueChanged;


        //组合控件
        private SliderInline _sliderInline;
        private Button _upBtn;
        private Button _downBtn;

        private class SliderInline : BaseUI, IPointerDragHandler
        {
            protected GUIStyle VerticalScrollberStyle => GUI.skin.verticalScrollbarThumb;

            public VerticalScrollbar Scrollbar { set; get; }

            public Action<Vector2> OnPositionChange;

            private Vector2 _lastPosition;

            protected override void OnDrawElement()
            {
                DrawStyle(VerticalScrollberStyle);
            }

            public void RefreshPosition()
            {
                if (AnchoredPosition.y < 16)
                {
                    AnchoredPosition = new Vector2(AnchoredPosition.x, 16);
                }
                else if (AnchoredPosition.y + RectInfo.height > Scrollbar.RectInfo.height - 16)
                {
                    AnchoredPosition = new Vector2(AnchoredPosition.x,
                        Scrollbar.RectInfo.height - RectInfo.height - 16);
                }

                if (_lastPosition != AnchoredPosition)
                {
                    _lastPosition = AnchoredPosition;
                    OnPositionChange?.Invoke(_lastPosition);
                }
            }

            public void OnDrag(Vector2 delta, Vector2 mousePos)
            {
                AnchoredPosition += new Vector2(0, delta.y);


                RefreshPosition();
            }

            public void OnStartDrag(Vector2 mousePos)
            {
                
            }

            public void OnDragEnd(Vector2 mousePos)
            {
            }
        }

        /// <summary>
        /// 设置内部滑动条的高度
        /// </summary>
        /// <param name="height"></param>
        public void SetSliderHeight(float height)
        {
            if (height > RectInfo.height - 32)
            {
                height = RectInfo.height - 32;
            }

            _sliderInline.Size = new Vector2(13, height);
        }

        /// <summary>
        /// 设置内部滑动条高度 使用相对
        /// </summary>
        /// <param name="height"></param>
        public void SetSliderHeightRelative(float height)
        {
            if (height < 0)
            {
                height = 0;
            }
            else if (height > 1)
            {
                height = 1;
            }


            _sliderInline.Size = new Vector2(13, height * (RectInfo.height - 32));
        }

        public void ScrollValue(float value)
        {
            _sliderInline.AnchoredPosition += new Vector2(0, value * (RectInfo.height - 32));
            _sliderInline.RefreshPosition();
        }

        public void SetValue(float value)
        {
            _sliderInline.AnchoredPosition =
                new Vector2(0, value * (RectInfo.height) - _sliderInline.RectInfo.height - 16);
            _sliderInline.RefreshPosition();
        }


        public override void OnLayerBuild()
        {
            Size = new Vector2(13, 100);

            _upBtn = CurEuiCore.CreateElement<Button>();
            _upBtn.Style = VertticalButtonUpStyle;
            _upBtn.SetAnchor(AnchorType.MiddleTop);
            _upBtn.Size = new Vector2(13, 16);
            _upBtn.Pivot = new Vector2(0.5f, 0);
            _upBtn.SetParent(this);
            _upBtn.OnClickEvt += () => { ScrollValue(-0.1f); };

            _downBtn = CurEuiCore.CreateElement<Button>();
            _downBtn.Style = VerticalButtonDownStyle;
            _downBtn.SetAnchor(AnchorType.MiddleBottom);
            _downBtn.Pivot = new Vector2(0.5f, 1);
            _downBtn.Size = new Vector2(13, 16);
            _downBtn.SetParent(this);
            _downBtn.OnClickEvt += () => { ScrollValue(0.1f); };


            _sliderInline = CurEuiCore.CreateElement<SliderInline>();
            _sliderInline.Scrollbar = this;
            _sliderInline.Size = new Vector2(13, 50);
            _sliderInline.Pivot = new Vector2(0.5f, 0);
            _sliderInline.SetAnchor(AnchorType.MiddleTop);
            _sliderInline.SetParent(this);
            _sliderInline.OnPositionChange += (x) =>
            {
                Value = (x.y - 16) / (RectInfo.height - _sliderInline.RectInfo.height - 32);
            };

            SetValue(0);
        }


        protected override void OnDrawElement()
        {
//            GUI.VerticalScrollbar(RenderRectInfo,0,10,0,10);
//            VerticalScrollbar_(RenderRectInfo, 0, 10, 0, 10);
//            EditorGUI.DrawRect(RenderRectInfo, new Color(0.5f, 0.5f, 0.5f, 0.8f));
            Rect r = RenderRectInfo;
            r.height -= 32;
            r.y += 16;
            VerticalSliderStyle.Draw(r, Text, IsHover, IsPress, false, Focus);

            ;

//            DrawSlider(VerticalScrollberStyle, Vector2.zero, new Vector2(RenderRectInfo.width, RenderRectInfo.height));
//            DrawSlider(VerticalSliderStyle, Vector2.zero, new Vector2(RenderRectInfo.width, RenderRectInfo.height - 10),
//                IsHover, IsPress, false, Focus);
        }

        public void VerticalScrollbar_(
            Rect position,
            float value,
            float size,
            float topValue,
            float bottomValue)
        {
            ScrollInline(position, value, size, topValue, bottomValue, GUI.skin.verticalScrollbar,
                GUI.skin.verticalScrollbarThumb, GUI.skin.verticalScrollbarUpButton,
                GUI.skin.verticalScrollbarDownButton, false);
        }

        void ScrollInline(Rect position,
            float value,
            float size,
            float leftValue,
            float rightValue,
            GUIStyle slider,
            GUIStyle thumb,
            GUIStyle leftButton,
            GUIStyle rightButton,
            bool horiz)
        {
            Rect position1;
            Rect rect1;
            Rect rect2;
            if (horiz)
            {
                position1 = new Rect(position.x + leftButton.fixedWidth, position.y,
                    position.width - leftButton.fixedWidth - rightButton.fixedWidth, position.height);
                rect1 = new Rect(position.x, position.y, leftButton.fixedWidth, position.height);
                rect2 = new Rect(position.xMax - rightButton.fixedWidth, position.y, rightButton.fixedWidth,
                    position.height);
            }
            else
            {
                position1 = new Rect(position.x, position.y + leftButton.fixedHeight, position.width,
                    position.height - leftButton.fixedHeight - rightButton.fixedHeight);
                rect1 = new Rect(position.x, position.y, position.width, leftButton.fixedHeight);
                rect2 = new Rect(position.x, position.yMax - rightButton.fixedHeight, position.width,
                    rightButton.fixedHeight);
            }


            slider.Draw(position1, IsHover, false, false, false);
            thumb.Draw(position1, IsHover, false, false, false);
        }
    }
}