using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorWindowEUI
{
    public interface IScrollHandler
    {
        void OnScroll(float delta);
    }
}