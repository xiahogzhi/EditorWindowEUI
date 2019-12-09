using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI
{
    public class EUICore
    {
        private List<UIElement> _rootElements = new List<UIElement>();

        /// <summary>
        /// 点进入事件需要的元素索引
        /// </summary>
        private int _pointerEnterIndex = -1;

        /// <summary>
        /// 点击事件需要的进入元素索引
        /// </summary>
        private int _pointerClickEnterIndex = -1;

        /// <summary>
        /// 点击事件需要的按下元素索引
        /// </summary>
        private int _pointerClickPressIndex = -1;

        /// <summary>
        /// 点击事件需要的鼠标id
        /// </summary>
        private int _pointerClickPressButton = 0;

        /// <summary>
        /// 拖拽元素索引
        /// </summary>
        private int _pointerDragIndex = -1;


        /// <summary>
        /// 上次拖拽时的鼠标位置
        /// </summary>
        private Vector2 _lastDragMousePosition;

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private bool _refreshWindow;

        /// <summary>
        /// 上次鼠标移动的位置
        /// </summary>
        private Vector2 _lastMoveMousePosition;

        /// <summary>
        /// 是否改变了元素,设置父节点,删除等操作
        /// </summary>
        private bool _hasChangedElement;

        /// <summary>
        /// 所有需要渲染的元素列表
        /// </summary>
        private readonly List<UIElement> _uiElements = new List<UIElement>();

        /// <summary>
        /// 遍历根源元素列表时临时装填的栈
        /// </summary>
        private readonly Stack<UIElement> _uiElementsStackTemp = new Stack<UIElement>();

        /// <summary>
        /// 焦点功能需要的列表,保存当前存在的焦点元素
        /// </summary>
        private List<IFocus> _focusElements = new List<IFocus>();

        /// <summary>
        /// 焦点功能需要的列表,用于添加新的焦点元素
        /// </summary>
        private List<IFocus> _focusElementsCur = new List<IFocus>();


        /// <summary>
        /// 当前窗口,自动注入
        /// </summary>
        public BaseEditorWindowGUI CurEditorWindow { private set; get; }

        private int _nextFrame;


        private PropertyInfo _curCoreProperty;

        public EUICore()
        {
            _curCoreProperty = typeof(UIElement).GetProperty("CurEuiCore");
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateElement<T>() where T : UIElement, new()
        {
            T t = new T();
            _curCoreProperty.SetValue(t, this);
            AddElement(t);
            t.OnLayerBuild();
            return t;
        }

        /// <summary>
        /// 设置根节点索引
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(int index, UIElement ui)
        {
            if (_rootElements.Contains(ui))
            {
                _rootElements.Remove(ui);

                if (index >= _rootElements.Count)
                    index = _rootElements.Count - 1;

                _rootElements.Insert(index, ui);
                RefreshAllElement();
            }
        }

        /// <summary>
        /// 获取根节点索引
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public int GetIndex(UIElement ui)
        {
            return _rootElements.IndexOf(ui);
        }

        /// <summary>
        /// 添加根源元素,不需要手动进行调用
        /// </summary>
        /// <param name="ui"></param>
        public void AddElement(UIElement ui)
        {
            _rootElements.Add(ui);
            RefreshAllElement();
        }

        /// <summary>
        /// 删除根源元素,不需要手动进行调用
        /// </summary>
        /// <param name="ui"></param>
        public void RemoveElement(UIElement ui)
        {
            _rootElements.Remove(ui);
            RefreshAllElement();
        }


        /// <summary>
        /// 刷新所有元素,会在每帧末尾进行统一刷新
        /// </summary>
        public void RefreshAllElement()
        {
            _hasChangedElement = true;
        }

        /// <summary>
        /// 刷新所有UI内部实现,层级排序
        /// </summary>
        private void RefreshAllElementInline()
        {
            _uiElements.Clear();

            foreach (var root in _rootElements)
            {
                _uiElementsStackTemp.Push(root);
                while (_uiElementsStackTemp.Count > 0)
                {
                    var ui = _uiElementsStackTemp.Pop();
                    _uiElements.Add(ui);

                    for (int i = ui.ChildCount - 1; i >= 0; i--)
                    {
                        _uiElementsStackTemp.Push(ui.GetChild(i));
                    }
                }
            }
        }


        /// <summary>
        /// 渲染当前添加的所有元素,在OnGUI进行调用,默认自动进行调用
        /// </summary>
        public void OnGUI()
        {
            Event evt = Event.current;

            if (evt.type == EventType.KeyDown && evt.control && evt.keyCode == KeyCode.R)
            {
                AssetDatabase.Refresh();
                evt.Use();
            }

            //先进行渲染
            if (evt.type == EventType.Repaint)
            {
                foreach (var VARIABLE in _uiElements)
                {
                    VARIABLE.Draw();
                }
            }

            //实现局部事件
            {
                Vector2 mousePos = evt.mousePosition;

                //Drag
                if (_pointerDragIndex != -1 && mousePos != _lastDragMousePosition)
                {
                    IPointerDragHandler drag = _uiElements[_pointerDragIndex] as IPointerDragHandler;
                    if (drag != null)
                    {
                        drag.OnDrag(mousePos - _lastDragMousePosition, mousePos);
                        _lastDragMousePosition = mousePos;
                        _refreshWindow = true;
                    }
                }


                //PointerExit
                if (_pointerEnterIndex != -1)
                {
                    if (_uiElements.Count <= _pointerEnterIndex)
                    {
                        _pointerEnterIndex = -1;
                    }
                    else
                    {
                        IOverlapPoint ip = _uiElements[_pointerEnterIndex] as IOverlapPoint;
                        if (ip != null && !ip.OverlapPoint(mousePos, evt))
                        {
                            IPointerExitHandler ie = ip as IPointerExitHandler;
                            ie?.OnPointerExit();
                            _pointerEnterIndex = -1;
                        }
                    }
                }

                //PointerClickExit
                if (_pointerClickEnterIndex != -1)
                {
                    if (_uiElements.Count <= _pointerClickEnterIndex)
                    {
                        _pointerClickEnterIndex = -1;
                    }
                    else
                    {
                        IOverlapPoint ip = _uiElements[_pointerClickEnterIndex] as IOverlapPoint;
                        if (ip != null && !ip.OverlapPoint(mousePos, evt))
                        {
                            _pointerClickEnterIndex = -1;
                        }
                    }
                }

                //PointerEnter
                for (int i = _uiElements.Count - 1; i >= 0; i--)
                {
                    IPointerEnterHandler id = _uiElements[i] as IPointerEnterHandler;
                    if (id != null && id.OverlapPoint(mousePos, evt))
                    {
                        if (_pointerEnterIndex == -1 || _pointerEnterIndex < i)
                        {
                            if (_pointerEnterIndex != -1)
                            {
                                IPointerExitHandler ie = _uiElements[_pointerEnterIndex] as IPointerExitHandler;
                                ie?.OnPointerExit();
                            }

                            _pointerEnterIndex = i;
                            id.OnPonterEnter();
                            break;
                        }
                    }
                }

                //PointerClickEnter
                for (int i = _uiElements.Count - 1; i >= 0; i--)
                {
                    IPointerClickHandler id = _uiElements[i] as IPointerClickHandler;
                    if (id != null && id.OverlapPoint(mousePos, evt))
                    {
                        if (_pointerClickEnterIndex == -1 || _pointerClickEnterIndex < i)
                        {
                            _pointerClickEnterIndex = i;
                            break;
                        }
                    }
                }
            }


            if (evt.type == EventType.MouseDown)
            {
                Vector2 mousePos = evt.mousePosition;

                bool hasFocus = false;
                //PointerDown
                for (int i = _uiElements.Count - 1; i >= 0; i--)
                {
                    UIElement ui = _uiElements[i];
                    IOverlapPoint ip = ui as IOverlapPoint;

                    if (ip.OverlapPoint(mousePos, evt))
                    {
                        hasFocus = true;
                        IFocus focus = ui as IFocus;

                        IPointerDownHandler down = ui as IPointerDownHandler;

                        IPointerDragHandler drag = ui as IPointerDragHandler;

                        //Focus
                        if (focus != null)
                        {
                            //向上查找所有父亲
                            UIElement parent = ui;
                            while (parent != null)
                            {
                                focus = parent as IFocus;
                                if (focus != null)
                                {
                                    _focusElementsCur.Add(focus);
                                }

                                parent = parent.Parent;
                            }

                            //取消之前的
                            for (int i2 = _focusElements.Count - 1; i2 >= 0; i2--)
                            {
                                if (!_focusElementsCur.Contains(_focusElements[i2]))
                                {
                                    _focusElements[i2].Focus = false;
                                }
                            }


                            //聚焦现在的,从父亲到孩子
                            for (int i2 = _focusElementsCur.Count - 1; i2 >= 0; i2--)
                            {
                                _focusElementsCur[i2].Focus = true;
                            }

                            //交换之前与现在的列表,并清除现在
                            List<IFocus> temp = _focusElements;
                            _focusElements = _focusElementsCur;
                            _focusElementsCur = temp;
                            _focusElementsCur.Clear();

                            if (down == null && drag == null)
                            {
                                break;
                            }
                        }

                        if (drag != null && _pointerDragIndex == -1 && evt.button == 0)
                        {
                            _pointerDragIndex = i;
                            drag.OnStartDrag(mousePos);

                            _lastDragMousePosition = mousePos;

                            if (down == null)
                            {
                                break;
                            }
                        }

                        if (down != null)
                        {
                            //PointerDown
                            down.OnPointerDown(mousePos, evt.shift, evt.control, evt.alt);
                            break;
                        }
                    }
                }


                //PointerClickDown
                for (int i = _uiElements.Count - 1; i >= 0; i--)
                {
                    IPointerClickHandler id = _uiElements[i] as IPointerClickHandler;
                    if (id != null && id.OverlapPoint(mousePos, evt) && _pointerClickPressIndex == -1)
                    {
                        _pointerClickPressButton = evt.button;
                        _pointerClickPressIndex = i;
                        break;
                    }
                }

                //处理空白区域点击取消Focus
                if (!hasFocus)
                {
                    //取消焦点
                    for (int i2 = _focusElements.Count - 1; i2 >= 0; i2--)
                    {
                        _focusElements[i2].Focus = false;
                    }

                    _focusElements.Clear();
                }

                _refreshWindow = true;
            }

            if (evt.type == EventType.MouseUp)
            {
                Vector2 mousePos = evt.mousePosition;

                if (evt.button == 0)
                {
                    if (_pointerDragIndex != -1)
                    {
                        if (_uiElements[_pointerDragIndex] is IPointerDragHandler drag)
                        {
                            drag.OnDragEnd(mousePos);
                        }

                        _pointerDragIndex = -1;
                    }
                }

                //PointerUp
                for (int i = _uiElements.Count - 1; i >= 0; i--)
                {
                    IPointerUpHandler id = _uiElements[i] as IPointerUpHandler;
                    if (id != null && id.OverlapPoint(mousePos, evt))
                    {
                        id.OnPointerUp(mousePos);
                        break;
                    }
                }

                //PointerClickUp
                if (_pointerClickPressIndex != -1)
                {
                    if (_pointerClickEnterIndex != -1)
                    {
                        IPointerClickHandler ic = _uiElements[_pointerClickPressIndex] as IPointerClickHandler;
                        if (_pointerClickPressButton == evt.button)
                        {
                            if (_pointerClickPressIndex == _pointerClickEnterIndex)
                            {
                                ic?.OnClick(mousePos);
                            }
                            else
                            {
                                ic?.OnCancelClick(mousePos);
                            }
                        }
                        else
                        {
                            ic?.OnCancelClick(mousePos);
                        }

                        _pointerClickEnterIndex = -1;
                        _pointerClickPressIndex = -1;
                    }
                    else
                    {
                        IPointerClickHandler ic = _uiElements[_pointerClickPressIndex] as IPointerClickHandler;
                        ic?.OnCancelClick(mousePos);
                        _pointerClickEnterIndex = -1;
                        _pointerClickPressIndex = -1;
                    }
                }

                _refreshWindow = true;
            }

            if (evt.type == EventType.KeyDown)
            {
                if (evt.character != 0)
                {
                    for (int i = _uiElements.Count - 1; i >= 0; i--)
                    {
                        ICharacterInputHandler ic = _uiElements[i] as ICharacterInputHandler;
                        ic?.OnCharacterInput(evt.character);
                    }
                }

                for (int i = _uiElements.Count - 1; i >= 0; i--)
                {
                    IKeyDownHandler ui = _uiElements[i] as IKeyDownHandler;
                    ui?.OnKeyDown(evt.keyCode, evt.shift, evt.control, evt.alt);
                }

                evt.Use();

                _refreshWindow = true;
            }

            if (evt.type == EventType.KeyUp)
            {
                for (int i = _uiElements.Count - 1; i >= 0; i--)
                {
                    IKeyUpHandler ui = _uiElements[i] as IKeyUpHandler;
                    ui?.OnKeyUp(evt.keyCode);
                }

                evt.Use();

                _refreshWindow = true;
            }


            //RefreshAllElement
            if (_hasChangedElement)
            {
                RefreshAllElementInline();
                _hasChangedElement = false;
                _refreshWindow = true;
            }

            //鼠标移动重绘
            if (_lastMoveMousePosition != evt.mousePosition)
            {
                _refreshWindow = true;
                _lastDragMousePosition = evt.mousePosition;
            }

            //自动重绘界面
            if (_refreshWindow)
            {
                _refreshWindow = false;
                CurEditorWindow.Repaint();
            }
        }
    }
}