using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EditorWindowEUI
{
    public class LayoutUIElement : RectUIElement //, IPointerDragHandler
    {
        private Vector2 _anchoredPosition;
        private Vector2 _anchorMax = new Vector2(0.5f, 0.5f);
        private Vector2 _anchorMin = new Vector2(0.5f, 0.5f);
        private Vector2 _offsetMax;
        private Vector2 _offsetMin;
        private Vector2 _pivot = new Vector2(0.5f, 0.5f);
        private Vector2 _size = new Vector2(100, 100);
        private Vector2 _position;

        private bool EnabledChangeAnchoredPosition = true;
        private bool EnabledChangeAnchorMax = true;
        private bool EnabledChangeAnchorMin = true;
        private bool EnabledChangeOffsetMax = true;
        private bool EnabledChangeOffsetMin = true;
        private bool EnabledChangePivot = true;
        private bool EnabledChangeSize = true;


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

//        public void OnDrag(Vector2 delta, Vector2 mousePos)
//        {
//            AnchordPosition += delta;
//        }

        /// <summary>
        /// 渲染用此,点击事件作用RectInfo,此Rect计算了裁剪坐标
        /// </summary>
        public Rect RenderRectInfo
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

        public override void OnLayerBuild()
        {
            base.OnLayerBuild();
            CalcPosition();
        }

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

        void UpdateClipInChild()
        {
            LayoutUIElement par = Parent as LayoutUIElement;
            if (par == null)
                return;

            //判断父节点是是否开启子节点裁剪

            Stack<LayoutUIElement> stack = new Stack<LayoutUIElement>();
            for (int i = 0; i < ChildCount; i++)
            {
                stack.Push(GetChild(i) as LayoutUIElement);
            }

            while (stack.Count > 0)
            {
                LayoutUIElement ui = stack.Pop();
                if (par._isActiveClipInChild)
                    ui.ClipParent = par;
                else if (par.ClipParent != null)
                    ui.ClipParent = par.ClipParent;
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


        public virtual Vector2 AnchoredPosition
        {
            get { return _anchoredPosition; }
            set
            {
                if (EnabledChangeAnchoredPosition && _anchoredPosition != value)
                {
                    _anchoredPosition = value;
                    CalcPosition();
                }
            }
        }

        public virtual Vector2 Size
        {
            get { return _size; }
            set
            {
                if (EnabledChangeSize && _size != value)
                {
                    _size = value;
                    CalcPosition();
                }
            }
        }

        public virtual float Width
        {
            get { return _size.x; }
            set
            {
                if (EnabledChangeSize && value != _size.x)
                {
                    _size.x = value;
                    CalcPosition();
                }
            }
        }
        
        public virtual float Height
        {
            get { return _size.y; }
            set
            {
                if (EnabledChangeSize && value != _size.y)
                {
                    _size.y = value;
                    CalcPosition();
                }
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            private set
            {
                if (_position != value)
                {
                    _position = value;
                    CalcPosition();
                }
            }
        }

        public virtual Vector2 AnchorMax
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
                if (EnabledChangeAnchorMax && _anchorMax != value)
                {
                    _anchorMax = value;
                    CalcPosition();
                }
            }
        }

        public virtual Vector2 AnchorMin
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
                if (EnabledChangeAnchorMin && _anchorMin != value)
                {
                    _anchorMin = value;
                    CalcPosition();
                }
            }
        }

        /// <summary>
        /// 自适应锚点类型时需要的最大参数
        /// </summary>
        public virtual Vector2 OffsetMax
        {
            get { return _offsetMax; }
            set
            {
                if (EnabledChangeOffsetMax && _offsetMax != value)
                {
                    _offsetMax = value;
                    CalcPosition();
                }
            }
        }

        /// <summary>
        /// 自适应锚点类型时需要的最小参数
        /// </summary>
        public virtual Vector2 OffsetMin
        {
            get { return _offsetMin; }
            set
            {
                if (EnabledChangeOffsetMin && _offsetMin != value)
                {
                    _offsetMin = value;
                    CalcPosition();
                }
            }
        }

        /// <summary>
        /// 轴点
        /// </summary>
        public virtual Vector2 Pivot
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

                if (EnabledChangePivot && _pivot != value)
                {
                    _pivot = value;
                    CalcPosition();
                }
            }
        }

        /// <summary>
        /// 设置锚点
        /// </summary>
        /// <param name="anchorType"></param>
        public virtual void SetAnchor(AnchorType anchorType)
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
                case AnchorType.HorizontalStretch:
                    _anchorMax = new Vector2(1, 0.5f);
                    _anchorMin = new Vector2(0, 0.5f);
                    break;
                case AnchorType.VerticalStretch:
                    _anchorMax = new Vector2(0.5f, 1);
                    _anchorMin = new Vector2(0.5f, 0);
                    break;
            }

            CalcPosition();
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

            if (parent == Parent)
                return;

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

            CalcPosition();

            UpdateClipInChild();
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            CalcPosition();
        }

        private void CalcPosition()
        {
            if (CurEuiCore == null)
                return;

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
                              _anchoredPosition.x;
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
                              _anchoredPosition.y;
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