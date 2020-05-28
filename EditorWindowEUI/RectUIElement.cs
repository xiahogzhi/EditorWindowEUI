using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EditorWindowEUI
{
    public class RectUIElement : UIElement, IFocus
    {
        /// <summary>
        /// 是否穿透,如果设置则忽略所有事件
        /// </summary>
        public bool IsPenetrate { set; get; }

        /// <summary>
        /// 当前物体实在的位置,用于检测点击,绘制基础形状
        /// </summary>
        public virtual Rect RectInfo { protected set; get; }


        private bool _focus;


        /// <summary>
        /// 焦点值,只有实现了IFocus,Windows才会进行设置.
        /// </summary>
        public bool Focus
        {
            get { return _focus; }
            set
            {
                if (_focus != value)
                {
                    _focus = value;
                    OnFocusChanged();
                }
            }
        }

        /// <summary>
        /// 当焦点改变时调用.
        /// </summary>
        protected virtual void OnFocusChanged()
        {
        }


        protected override void OnDraw()
        {
            //  EditorGUI.DrawRect(RectInfo, new Color(0.3f,0.8f,0.6f,0.1f));
        }

        public virtual bool OverlapPoint(Vector2 point, Event curEvt)
        {
            return IsVisibility && RectInfo.Contains(point) && !IsPenetrate;
        }
    }
}