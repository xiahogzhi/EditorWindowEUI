using UnityEngine;

namespace EditorWindowEUI
{
    public interface IPointerEnterHandler : IOverlapPoint
    {
        void OnPointerEnter();
    }
}