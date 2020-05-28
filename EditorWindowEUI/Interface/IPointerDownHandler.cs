using UnityEngine;

namespace EditorWindowEUI
{
    public interface IPointerDownHandler : IOverlapPoint
    {
        void OnPointerDown(Vector2 pos,bool isShift,bool isCtrl,bool isAlt);
    }
}