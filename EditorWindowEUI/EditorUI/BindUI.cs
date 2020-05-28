using System;
using System.Collections;
using System.Collections.Generic;
using EditorWindowEUI.UI;
using UnityEngine;

namespace EditorWindowEUI.EditorUI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BindUI : Attribute
    {
        
        private Type _ui;

        public Type Ui => _ui;

        public BindUI(Type ui)
        {
            _ui = ui;
        }
    }
}