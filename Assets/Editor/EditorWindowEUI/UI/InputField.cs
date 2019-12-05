using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace EditorWindowEUI
{
    public class InputField : BaseUI, IKeyDownHandler, ICharacterInputHandler, IPointerDragHandler
    {
        private int _cursorIndex;

        private int _selectStart;

        private bool _isSelect;

        private Vector2 _offset;

        private GUIContent _content;

        private Vector2 _currentPosition;

        private int _id;

        private int _maxIndex;

        private PropertyInfo Internal_clipOffset;

        public override string Text
        {
            get { return _content.text; }
            set { _content.text = value; }
        }

        public InputField()
        {
            Internal_clipOffset =
                typeof(GUIStyle).GetProperty("Internal_clipOffset", BindingFlags.NonPublic | BindingFlags.Instance);
            _content = new GUIContent();
            _id = GUIUtility.GetControlID(FocusType.Keyboard);
        }

        public override void OnPointerDown(Vector2 pos,bool isShift,bool isCtrl,bool isAlt)
        {
            base.OnPointerDown(pos,isShift,isCtrl,isAlt);
            _isSelect = false;

            if (Focus)
            {
                Rect r = RectInfo;
                r.x += _offset.x;

                _cursorIndex = GetStyle().GetCursorStringIndex(r, _content, pos);
                if (!isShift)
                {
                    _selectStart = _cursorIndex;
                }
            }
        }

        protected override GUIStyle GetStyle()
        {
//            return AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Skin.guiskin").textField;
            return EditorStyles.textField;
        }

        protected override void OnUIDraw()
        {
            Rect rectInfo = RenderInfo;
            _currentPosition = GetStyle().GetCursorPixelPosition(rectInfo, _content, _cursorIndex);


            if (_currentPosition.x > rectInfo.xMax - 4 - _offset.x)
            {
                _offset.x = rectInfo.xMax - _currentPosition.x - 4;
            }
            else if (_currentPosition.x < rectInfo.xMin - _offset.x + 3)
            {
                _offset.x = rectInfo.xMin - _currentPosition.x + 3;
            }


            EditorGUIUtility.AddCursorRect(rectInfo, MouseCursor.Text);

            Vector2 content = GetStyle().contentOffset;
            GetStyle().contentOffset = new Vector2(_offset.x, 0);
            Internal_clipOffset.SetValue(GetStyle(), new Vector2(-_offset.x, 0));

            if (Focus)
            {
                GetStyle().DrawWithTextSelection(rectInfo, _content, _id,
                    _selectStart, _cursorIndex);

                GetStyle().DrawCursor(rectInfo, _content, _id, _cursorIndex);
            }
            else
            {
                GetStyle().Draw(rectInfo, _content, IsHover, false, false, false);
            }

            Internal_clipOffset.SetValue(GetStyle(), Vector2.zero);
            GetStyle().contentOffset = content;
        }

        protected override void OnFocusChanged()
        {
            if (Focus)
            {
                Input.imeCompositionMode = IMECompositionMode.On;
                GUIUtility.keyboardControl = _id;
            }
            else
            {
                Input.imeCompositionMode = IMECompositionMode.Off;
                GUIUtility.keyboardControl = 0;
            }
        }

  

        public void OnKeyDown(KeyCode k, bool isShift, bool isCtrl, bool isAlt)
        {
            if (Focus)
            {
                if (k == KeyCode.LeftArrow)
                {
                    if (_cursorIndex > 0)
                    {
                        _cursorIndex--;
                        if (!isShift)
                            _selectStart = _cursorIndex;
                    }
                }
                else if (k == KeyCode.X && isCtrl)
                {
                    int left;
                    int right;
                    if (_selectStart > _cursorIndex)
                    {
                        right = _selectStart;
                        left = _cursorIndex;
                    }
                    else
                    {
                        right = _cursorIndex;
                        left = _selectStart;
                    }


                    string copy = Text.Substring(left, right-1);

                    Text = Text.Substring(0, left) +
                           Text.Substring(right);
                    

                    EditorGUIUtility.systemCopyBuffer = copy;

                    _cursorIndex = left;
                    _selectStart = _cursorIndex;
                }
                else if (k == KeyCode.RightArrow)
                {
                    if (_cursorIndex < Text.Length)
                    {
                        _cursorIndex++;

                        if (!isShift)
                            _selectStart = _cursorIndex;
                    }
                }
                else if (k == KeyCode.C && isCtrl)
                {
                    int left;
                    int right;
                    if (_selectStart > _cursorIndex)
                    {
                        right = _selectStart;
                        left = _cursorIndex;
                    }
                    else
                    {
                        right = _cursorIndex;
                        left = _selectStart;
                    }


                    GUIUtility.systemCopyBuffer = Text.Substring(left, right - left);
                }
                else if (k == KeyCode.V && isCtrl)
                {
                    int left;
                    int right;
                    if (_selectStart > _cursorIndex)
                    {
                        right = _selectStart;
                        left = _cursorIndex;
                    }
                    else
                    {
                        right = _cursorIndex;
                        left = _selectStart;
                    }

                    if (!string.IsNullOrEmpty(GUIUtility.systemCopyBuffer))
                    {
                        _cursorIndex = left + GUIUtility.systemCopyBuffer.Length;
                        _selectStart = _cursorIndex;
                        Text = Text.Substring(0, left) + GUIUtility.systemCopyBuffer +
                               Text.Substring(right);
                    }

                   
                }
                else if (k == KeyCode.A && isCtrl)
                {
                    _selectStart = 0;
                    _cursorIndex = Text.Length;
                }
                else if (k == KeyCode.Backspace || k == KeyCode.Delete)
                {
                    int left;
                    int right;
                    if (_selectStart > _cursorIndex)
                    {
                        right = _selectStart;
                        left = _cursorIndex;
                    }
                    else
                    {
                        right = _cursorIndex;
                        left = _selectStart;
                    }

                    if (k == KeyCode.Backspace)
                    {
                        if (left == right && left > 0)
                        {
                            left--;
                        }
                    }
                    else if (k == KeyCode.Delete)
                    {
                        if (left == right && right < Text.Length)
                        {
                            right++;
                        }
                    }

                    Text = Text.Substring(0, left) +
                           Text.Substring(right);

                    _isSelect = false;
                    _cursorIndex = left;
                    _selectStart = _cursorIndex;


                    Vector2 curPos = GetStyle().GetCursorPixelPosition(RectInfo, _content, Text.Length);
                    Vector2 curPos2 = GetStyle().GetCursorPixelPosition(RectInfo, _content, _cursorIndex);
                    if (curPos.x < RectInfo.xMax - _offset.x && curPos2.x > RectInfo.xMax)
                    {
                        _offset.x = RectInfo.xMax - curPos.x - 4;
                    }
                    else if (curPos2.x < RectInfo.xMax)
                    {
                        _offset.x = 0;
                    }
                }
            }
        }


        public void OnCharacterInput(char c)
        {
            if (Focus)
            {
                if (c == '\n')
                {
                    c = ' ';
                }
                
                Text = Text.Insert(_cursorIndex, c + "");
                _cursorIndex++;
                _selectStart = _cursorIndex;
            }
        }

        public void OnDrag(Vector2 delta, Vector2 mousePos)
        {
            _cursorIndex = GetStyle().GetCursorStringIndex(RectInfo, _content, mousePos - _offset);
//            if (mousePos < )
//            {
//                
//            }
            _isSelect = true;
        }
    }
}