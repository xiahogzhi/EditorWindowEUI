using System;
using UnityEngine;

namespace EditorWindowEUI.UI
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

    

        public override void OnClick(Vector2 pos)
        {
            base.OnClick(pos);
            IsOn = !IsOn;
        }

        protected override void OnDrawElement()
        {
            GUI.skin.toggle.Draw(RenderRectInfo, Text, IsHover, IsPress, IsOn, Focus);
        }
    }
}