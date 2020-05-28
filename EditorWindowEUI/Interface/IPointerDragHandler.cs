using UnityEngine;

namespace EditorWindowEUI
{
    public interface IPointerDragHandler : IOverlapPoint
    {
        void OnDrag(Vector2 delta,Vector2 mousePos);

        void OnStartDrag(Vector2 mousePos);

        void OnDragEnd(Vector2 mousePos);
    }
}