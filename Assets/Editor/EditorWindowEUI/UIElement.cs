using System.Collections.Generic;

namespace EditorWindowEUI
{
    public class UIElement
    {
        public EUICore CurEuiCore { private set; get; }

        private string _name;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    return GetType().FullName;
                }
                else
                {
                    return _name;
                }
            }
            set { _name = value; }
        }

        private List<UIElement> _child = new List<UIElement>();


        public UIElement Parent { private set; get; }

        /// <summary>
        /// 子元素个数
        /// </summary>
        public int ChildCount => _child.Count;


        /// <summary>
        /// 获取子元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public UIElement GetChild(int index)
        {
            return _child[index];
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// 是否添加ui
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        protected virtual bool CanAddChild(UIElement ui)
        {
            return true;
        }

        /// <summary>
        /// 设置父母关系,如果参数为空则会设置为Root.
        /// </summary>
        /// <param name="parent"></param>
        public virtual void SetParent(UIElement parent)
        {
            if (!CanAddChild(this))
                return;

            if (Parent != parent)
            {
                if (Parent != null)
                {
                    Parent._child.Remove(this);
                }
                else
                {
                    CurEuiCore.RemoveElement(this);
                }


                if (parent == null)
                {
                    CurEuiCore.AddElement(this);
                }
                else
                {
                    Parent = parent;
                    parent._child.Add(this);
                    CurEuiCore.RefreshAllElement();
                }
            }
        }


        /// <summary>
        /// 由窗口的OnGUI进行调用,不需要手动调用.
        /// </summary>
        public void Draw()
        {
            OnDraw();
            foreach (var VARIABLE in _child)
            {
                VARIABLE.Draw();
            }
        }

        /// <summary>
        /// 每帧绘制时进行调用,在事件触发前进行调用.
        /// </summary>
        protected virtual void OnDraw()
        {
        }
    }
}