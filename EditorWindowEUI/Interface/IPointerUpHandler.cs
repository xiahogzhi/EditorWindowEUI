using UnityEngine;

namespace EditorWindowEUI
{
    public interface IPointerUpHandler : IOverlapPoint
    {
        void OnPointerUp(Vector2 pos);
    }
}