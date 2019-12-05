using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI
{
    public class Image : BaseUI,IPointerDragHandler
    {
        private Texture2D _mainTexture;

        public Texture2D MainTexture
        {
            get => _mainTexture;
            set => _mainTexture = value;
        }

        protected override GUIStyle GetStyle()
        {
            return null;
        }


        protected override void OnUIDraw()
        {
            EditorGUI.DrawTextureTransparent(RenderInfo, MainTexture);
        }

        public void OnDrag(Vector2 delta, Vector2 mousePos)
        {
            AnchordPosition += delta;
        }
    } 
}