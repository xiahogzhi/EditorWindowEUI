using System.Collections.Generic;

namespace EditorWindowEUI
{
    public class UIElement
    {
        private bool _isVisibility = true;

        public EUICore CurEuiCore { private set; get; }

        public object Param { set; get; }

        /// <summary>
        /// 创建时调用
        /// </summary>
        public virtual void OnLayerBuild()
        {
        }


        /// <summary>
        /// 是否可见
        /// </summary>
        public virtual bool IsVisibility
        {
            set => _isVisibility = value;
            get => _isVisibility;
        }

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


        protected virtual void OnAddChild(UIElement ui)
        {
        }

        protected virtual void OnRemoveChild(UIElement ui)
        {
        }

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
        /// 销毁当前UI
        /// </summary>
        public void Destroy()
        {
            OnDestroy();
            if (Parent != null)
            {
                Parent._child.Remove(this);
                Parent.OnRemoveChild(this);
            }
            else
            {
                CurEuiCore.RemoveElement(this);
            }

            CurEuiCore.RefreshAllElement();
        }

        /// <summary>
        /// 销毁时调用
        /// </summary>
        protected virtual void OnDestroy()
        {
        }

        /// <summary>
        /// 获取当前索引
        /// </summary>
        /// <returns></returns>
        public int GetIndex()
        {
            if (Parent == null)
            {
                return CurEuiCore.GetIndex(this);
            }

            return Parent._child.IndexOf(this);
        }

        /// <summary>
        /// 设置索引
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(int index)
        {
            if (index < 0)
                index = 0;

            if (Parent == null)
            {
                CurEuiCore.SetIndex(index, this);
                return;
            }

            Parent._child.Remove(this);

            if (index >= Parent.ChildCount)
                index = Parent.ChildCount - 1;

            Parent._child.Insert(index, this);

            CurEuiCore.RefreshAllElement();
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
                    Parent.OnRemoveChild(this);
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
                    Parent.OnAddChild(this);
                    CurEuiCore.RefreshAllElement();
                }
            }
        }


        /// <summary>
        /// 由窗口的OnGUI进行调用,不需要手动调用.
        /// </summary>
        public void Draw()
        {
            if (IsVisibility)
            {
                OnDraw();
                foreach (var VARIABLE in _child)
                {
                    VARIABLE.Draw();
                }
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