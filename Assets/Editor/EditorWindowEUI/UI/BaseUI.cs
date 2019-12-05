using UnityEngine;

namespace EditorWindowEUI
{
    public abstract class BaseUI : LayoutUIElement, IPointerEnterHandler,
        IPointerExitHandler, IPointerClickHandler
    {
        public virtual bool IsHover { set; get; }

        public virtual bool IsActive { set; get; }

        public virtual bool Enabled { set; get; }

        public virtual string Text { set; get; }

        protected abstract GUIStyle GetStyle();

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
            OnUIDraw();
            GUI.enabled = value;

            if (ClipParent != null)
            {
                GUI.EndClip();
            }
        }

        protected virtual void OnUIDraw()
        {
            GetStyle().Draw(RenderInfo, Text, IsHover, IsActive, false, Focus);
        }

        public virtual void OnPointerDown(Vector2 pos, bool isShift, bool isCtrl, bool isAlt)
        {
            IsActive = true;
        }

        public virtual void OnPointerUp(Vector2 pos)
        {
            IsActive = false;
        }

        public virtual void OnClick(Vector2 pos)
        {
            IsActive = false;
        }

        public virtual void OnCancelClick(Vector2 pos)
        {
            IsActive = false;
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