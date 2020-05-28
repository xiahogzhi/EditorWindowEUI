using UnityEngine;

namespace EditorWindowEUI
{
    public interface IPointerClickHandler : IPointerDownHandler, IPointerUpHandler
    {
        void OnClick(Vector2 pos);

        void OnCancelClick(Vector2 pos);
    }
}