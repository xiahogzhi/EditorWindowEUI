using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.UI
{
    public class Image : BaseUI
    {
        private Texture2D _mainTexture;

        public Texture2D MainTexture
        {
            get => _mainTexture;
            set => _mainTexture = value;
        }

        
        protected override void OnDrawElement()
        {
            if (MainTexture)
                EditorGUI.DrawTextureTransparent(RenderRectInfo, MainTexture);    
            
        }

        public void OnDrag(Vector2 delta, Vector2 mousePos)
        {
            AnchoredPosition += delta;
        }
    } 
}