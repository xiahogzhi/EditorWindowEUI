using UnityEngine;

namespace EditorWindowEUI
{
    /// <summary>
    /// 点是否落在范围内,用于点击处理
    /// </summary>
    public interface IOverlapPoint
    {
        bool OverlapPoint(Vector2 point, Event curEvt);
    }
}