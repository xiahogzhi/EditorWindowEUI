using System;
using UnityEngine;

namespace EditorWindowEUI
{
    public class Toggle : BaseUI
    {
        private bool _isOn;

        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (_isOn != value)
                {
                    _isOn = value;
                    OnValueChangedEvt?.Invoke(value);
                }
            }
        }


        public event Action<bool> OnValueChangedEvt;

        protected override GUIStyle GetStyle()
        {
            return GUI.skin.toggle;
        }


        public override void OnClick(Vector2 pos)
        {
            base.OnClick(pos);
            IsOn = !IsOn;
        }

        protected override void OnUIDraw()
        {
            GetStyle().Draw(RenderInfo, Text, IsHover, IsActive, IsOn, Focus);
        }
    }
}