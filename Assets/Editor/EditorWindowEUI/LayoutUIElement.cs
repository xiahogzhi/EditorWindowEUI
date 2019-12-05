using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EditorWindowEUI
{
    public class LayoutUIElement : RectUIElement
    {
        private Vector2 _anchordPosition;
        private Vector2 _anchorMax;
        private Vector2 _anchorMin;
        private Vector2 _offsetMax;
        private Vector2 _offsetMin;
        private Vector2 _pivot;
        private Vector2 _size;
        private Vector2 _position;

        /// <summary>
        /// 裁剪父物体,如果存在则表示使用父物体的RectInfo作为裁剪区域
        /// </summary>
        protected RectUIElement ClipParent { private set; get; }

        public override bool OverlapPoint(Vector2 point, Event curEvt)
        {
            //如果使用裁剪则判断是否在裁剪范围内
            if (ClipParent != null)
            {
                return base.OverlapPoint(point, curEvt) && ClipParent.RectInfo.Contains(point);
            }

            return base.OverlapPoint(point, curEvt);
        }

        /// <summary>
        /// 渲染用此,点击事件作用RectInfo,此Rect计算了裁剪坐标
        /// </summary>
        public Rect RenderInfo
        {
            get
            {
                if (ClipParent != null)
                {
                    return new Rect(RectInfo.position - ClipParent.RectInfo.position, RectInfo.size);
                }

                return RectInfo;
            }
        }

        //用于判断当前ui是否开启裁剪子物体
        private bool _isActiveClipInChild;


        /// <summary>
        /// 是否开启裁剪
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActiveClip(bool isActive)
        {
            //如果当前物体已经被裁剪了则不需要裁剪子物体
            if (ClipParent != null)
                return;

            if (_isActiveClipInChild == isActive)
            {
                return;
            }

            _isActiveClipInChild = isActive;
            Stack<LayoutUIElement> stack = new Stack<LayoutUIElement>();
            for (int i = 0; i < ChildCount; i++)
            {
                stack.Push(GetChild(i) as LayoutUIElement);
            }

            while (stack.Count > 0)
            {
                LayoutUIElement ui = stack.Pop();
                if (isActive)
                    ui.ClipParent = this;
                else
                    ui.ClipParent = null;

                //如果当前子物体开启裁剪则取消
                ui._isActiveClipInChild = false;

                for (int i = 0; i < ui.ChildCount; i++)
                {
                    stack.Push(ui.GetChild(i) as LayoutUIElement);
                }
            }
        }


        public LayoutUIElement()
        {
            AnchordPosition = Vector2.zero;
            Pivot = new Vector2(0.5f, 0.5f);
            SetAnchor(AnchorType.MiddleCenter);
            Size = new Vector2(100, 100);
        }

        public Vector2 AnchordPosition
        {
            get { return _anchordPosition; }
            set { _anchordPosition = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            private set { _position = value; }
        }

        public Vector2 AnchorMax
        {
            get { return _anchorMax; }
            set
            {
                if (value.x < 0)
                    value.x = 0;

                if (value.x > 1)
                    value.x = 1;

                if (value.y < 0)
                    value.y = 0;

                if (value.y > 1)
                    value.y = 1;
                _anchorMax = value;
            }
        }

        public Vector2 AnchorMin
        {
            get { return _anchorMin; }
            set
            {
                if (value.x < 0)
                    value.x = 0;

                if (value.x > 1)
                    value.x = 1;

                if (value.y < 0)
                    value.y = 0;

                if (value.y > 1)
                    value.y = 1;
                _anchorMin = value;
            }
        }

        /// <summary>
        /// 自适应锚点类型时需要的最大参数
        /// </summary>
        public Vector2 OffsetMax
        {
            get { return _offsetMax; }
            set { _offsetMax = value; }
        }

        /// <summary>
        /// 自适应锚点类型时需要的最小参数
        /// </summary>
        public Vector2 OffsetMin
        {
            get { return _offsetMin; }
            set { _offsetMin = value; }
        }

        /// <summary>
        /// 轴点
        /// </summary>
        public Vector2 Pivot
        {
            get { return _pivot; }
            set
            {
                if (value.x < 0)
                    value.x = 0;

                if (value.x > 1)
                    value.x = 1;

                if (value.y < 0)
                    value.y = 0;

                if (value.y > 1)
                    value.y = 1;

                _pivot = value;
            }
        }

        /// <summary>
        /// 设置锚点
        /// </summary>
        /// <param name="anchorType"></param>
        public void SetAnchor(AnchorType anchorType)
        {
            switch (anchorType)
            {
                case AnchorType.MiddleCenter:
                    _anchorMax = new Vector2(0.5f, 0.5f);
                    _anchorMin = new Vector2(0.5f, 0.5f);
                    break;
                case AnchorType.MiddleTop:
                    _anchorMax = new Vector2(0.5f, 1);
                    _anchorMin = new Vector2(0.5f, 1);
                    break;
                case AnchorType.MiddleBottom:
                    _anchorMax = new Vector2(0.5f, 0);
                    _anchorMin = new Vector2(0.5f, 0);
                    break;
                case AnchorType.LeftCenter:
                    _anchorMax = new Vector2(0, 0.5f);
                    _anchorMin = new Vector2(0, 0.5f);
                    break;
                case AnchorType.LeftTop:
                    _anchorMax = new Vector2(0, 1);
                    _anchorMin = new Vector2(0, 1);
                    break;
                case AnchorType.LeftBottom:
                    _anchorMax = new Vector2(0, 0);
                    _anchorMin = new Vector2(0, 0);
                    break;
                case AnchorType.RightCenter:
                    _anchorMax = new Vector2(1, 0.5f);
                    _anchorMin = new Vector2(1, 0.5f);
                    break;
                case AnchorType.RightTop:
                    _anchorMax = new Vector2(1, 1);
                    _anchorMin = new Vector2(1, 1);
                    break;
                case AnchorType.RightBottom:
                    _anchorMax = new Vector2(1, 0);
                    _anchorMin = new Vector2(1, 0);
                    break;
                case AnchorType.Stretch:
                    _anchorMax = new Vector2(1, 1);
                    _anchorMin = new Vector2(0, 0);
                    break;
                case AnchorType.LeftStretch:
                    _anchorMax = new Vector2(0, 1);
                    _anchorMin = new Vector2(0, 0);
                    break;
                case AnchorType.TopStretch:
                    _anchorMax = new Vector2(1, 1);
                    _anchorMin = new Vector2(0, 1);
                    break;
                case AnchorType.BottomStretch:
                    _anchorMax = new Vector2(1, 0);
                    _anchorMin = new Vector2(0, 0);
                    break;

                case AnchorType.RightStretch:
                    _anchorMax = new Vector2(1, 1);
                    _anchorMin = new Vector2(1, 0);
                    break;
            }
        }


        protected override bool CanAddChild(UIElement ui)
        {
            if (!(ui is LayoutUIElement))
            {
                Debug.Log("该布局只能添加继承了LayoutUIElement的元素!");
                return false;
            }

            return true;
        }

        public override void SetParent(UIElement parent)
        {
            if (parent != null && !(parent is LayoutUIElement))
            {
                Debug.Log("该布局只能添加继承了LayoutUIElement的元素!");
                return;
            }

            LayoutUIElement ui = parent as LayoutUIElement;
            if (ui.ClipParent != null)
            {
                ClipParent = ui.ClipParent;
            }
            else if (ui._isActiveClipInChild)
            {
                ClipParent = ui;
            }


            base.SetParent(parent);
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            CalcPosition();
        }

        public void CalcPosition()
        {
            Rect parentRect = Rect.zero;
            Rect _result = RectInfo;
            LayoutUIElement parent = Parent as LayoutUIElement;
            if (parent != null)
            {
                parentRect = parent.RectInfo;
            }
            else
            {
                parentRect = CurEuiCore.CurEditorWindow.position;
                parentRect.position = Vector2.zero;
            }

            if (AnchorMax.x - AnchorMin.x == 0) //不拉伸
            {
                _position.x = parentRect.x + AnchorMax.x * parentRect.width - Size.x * Pivot.x +
                              _anchordPosition.x;
                _result.width = Size.x;
            }
            else
            {
                _position.x = parentRect.x + _offsetMin.x;
                _result.width = parentRect.width - _offsetMax.x - _offsetMin.x;
            }

            if (AnchorMax.y - AnchorMin.y == 0)
            {
                _position.y = parentRect.y + (1 - AnchorMax.y) * parentRect.height - Size.y * Pivot.y +
                              _anchordPosition.y;
                _result.height = Size.y;
            }
            else
            {
                _position.y = parentRect.y + _offsetMin.y;
                _result.height = parentRect.height - _offsetMax.y - _offsetMin.y;
            }

            _result.position = Position;
            RectInfo = _result;
        }
    }
}