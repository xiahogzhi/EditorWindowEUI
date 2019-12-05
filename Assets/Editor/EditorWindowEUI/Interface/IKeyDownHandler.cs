using UnityEngine;

namespace EditorWindowEUI
{
    public interface IKeyDownHandler
    {
        void OnKeyDown(KeyCode k, bool isShift, bool isCtrl,bool isAlt);
    }
}