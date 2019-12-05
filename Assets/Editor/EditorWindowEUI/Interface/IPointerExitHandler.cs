using UnityEngine;

namespace EditorWindowEUI
{
    public interface IPointerExitHandler : IOverlapPoint
    {
        void OnPointerExit();
    }
}