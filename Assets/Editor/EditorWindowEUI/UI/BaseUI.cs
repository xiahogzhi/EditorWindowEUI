using UnityEngine;

namespace EditorWindowEUI.UI
{
    public abstract class BaseUI : LayoutUIElement, IPointerEnterHandler,
        IPointerExitHandler, IPointerClickHandler
    {
        protected virtual bool IsHover { set; get; }

        protected virtual bool IsPress { set; get; }

        public bool Enabled { set; get; }

        public virtual string Text { set; get; }

        public BaseUI()
        {
            Enabled = true;
        }

        protected override void OnDraw()
        {
            base.OnDraw();


            if (ClipParent != null)
            {
                GUI.BeginClip(ClipParent.RectInfo);
            }

            bool value = GUI.enabled;
            GUI.enabled = Enabled;
            OnDrawElement();
            GUI.enabled = value;

            if (ClipParent != null)
            {
                GUI.EndClip();
            }
        }

        /// <summary>
        /// 绘制UI
        /// </summary>
        protected abstract void OnDrawElement();

        protected virtual void DrawStyle(GUIStyle gs)
        {
            gs.Draw(RenderRectInfo, Text, IsHover, IsPress, false, Focus);
        }


        public virtual void OnPointerDown(Vector2 pos, bool isShift, bool isCtrl, bool isAlt)
        {
            IsPress = true;
        }

        public virtual void OnPointerUp(Vector2 pos)
        {
            IsPress = false;
        }

        public virtual void OnClick(Vector2 pos)
        {
            IsPress = false;
        }

        public virtual void OnCancelClick(Vector2 pos)
        {
            IsPress = false;
        }

        public virtual void OnPonterEnter()
        {
            IsHover = true;
        }

        public virtual void OnPointerExit()
        {
            IsHover = false;
        }
    }
}