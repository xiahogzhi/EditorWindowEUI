using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI.UI
{
    public class TimeAxis : BaseUI, IPointerDragHandler
    {
        private int _cursorOffsetX;

        private int _minOffsetX;

        private int _timeInvert = 30;

        private int _highInvert = 10;

        private int _autoMoveSpeed = 3;

        private int _autoMove;

        public override void OnLayerBuild()
        {
            base.OnLayerBuild();
            Size = new Vector2(650, 30);
        }

        protected override void OnDrawElement()
        {
            Rect render = RenderRectInfo;
            //绘制背景
            EditorGUI.DrawRect(render, new Color(0.3f, 0.3f, 0.3f, 1f));

            //绘制帧显示
            int highIndex = 0;
            for (int i = 0; i <= _minOffsetX + render.width; i++)
            {
                int index = i - _minOffsetX;
                if ((index) % _timeInvert == 0)
                {
                    highIndex++;
                    if (highIndex > _highInvert)
                    {
                        highIndex = 0;
                        if (i >= _minOffsetX)
                        {
                            EditorGUI.DrawRect(new Rect(render.x + index, render.y + 15, 2, 15),
                                new Color(0.7f, 0.7f, 0.7f, 1));
                        }
                    }
                    else
                    {
                        if (i >= _minOffsetX)
                        {
                            EditorGUI.DrawRect(new Rect(render.x + index, render.y + 20, 1, 10),
                                new Color(0.5f, 0.5f, 0.5f, 1));
                        }
                    }

                    if (i >= _minOffsetX)
                    {
                        EditorGUI.LabelField(new Rect(render.x + index - 2, render.y + 2, _timeInvert, 10),
                            ConvertPositionToIndex(i).ToString());
                    }
                }
            }


            float x = ((int) (_cursorOffsetX / (float)_timeInvert)) * _timeInvert;
            //绘制指针
            EditorGUI.DrawRect(new Rect(x + render.x, render.y, 2, 30),
                new Color(0.8f, 0.8f, 0.8f, 1));


            if (_autoMove != 0)
            {
                _minOffsetX += (int) _autoMove;
                if (_minOffsetX < 0)
                {
                    _minOffsetX = 0;
                }
            }
        }

        int ConvertPositionToIndex(float x)
        {
            int x2 = ((int) (x / _timeInvert)) * _timeInvert;
            int x3 = ((int) (_minOffsetX / (float) _timeInvert)) * _timeInvert;
            return (x2 + x3) / _timeInvert;
        }

        public override void OnPointerDown(Vector2 pos, bool isShift, bool isCtrl, bool isAlt)
        {
            base.OnPointerDown(pos, isShift, isCtrl, isAlt);

            _cursorOffsetX = (int) (pos.x - RenderRectInfo.xMin );

            Debug.Log(ConvertPositionToIndex(_cursorOffsetX + _minOffsetX));
        }

        public void OnDrag(Vector2 delta, Vector2 mousePos)
        {
            _cursorOffsetX += (int) delta.x;

            if (_cursorOffsetX > RectInfo.width)
            {
                _cursorOffsetX = (int) RectInfo.width;


//                _minOffsetX += (int) delta.x;
                _autoMove = _autoMoveSpeed;
            }
            else if (_cursorOffsetX < 0)
            {
                _cursorOffsetX = 0;
                _autoMove = -_autoMoveSpeed;
            }
            else
            {
                _autoMove = 0;
            }
        }

        public void OnStartDrag(Vector2 mousePos)
        {
        }

        public void OnDragEnd(Vector2 mousePos)
        {
            _autoMove = 0;
        }
    }
}