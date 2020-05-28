using System;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI
{
    public abstract class BaseEditorWindowGUI : EditorWindow
    {
        [SerializeField] private bool _isCompiling; //是否编译中
        [SerializeField] private bool _isPlayering; //是否在游玩
        [SerializeField] private bool _isInit;

        private EUICore _euiCore;

        public EUICore EuiCore => _euiCore;

        protected abstract void OnInitialize();

        public void LayoutBuild()
        {
            wantsMouseEnterLeaveWindow = true;
            wantsMouseMove = true;
            _euiCore = new EUICore();
            typeof(EUICore).GetProperty("CurEditorWindow")?.SetValue(_euiCore, this);

            OnLayoutBuild();
        }

        private bool _startLayoutBuildFlag;

        protected abstract void OnLayoutBuild();

        protected virtual void StartCompile()
        {
        }

        protected virtual void StartPlaying()
        {
        }

        protected virtual void EndPlaying()
        {
        }

        protected virtual void EndCompile()
        {
        }


        private void OnGUI()
        {
//            wantsMouseMove = true;
//            wantsMouseEnterLeaveWindow = true;


            if (_euiCore == null)
            {
                _startLayoutBuildFlag = true;
            }

            if (!_isInit)
            {
                _isInit = true;
                OnInitialize();
                _startLayoutBuildFlag = true;
            }


            if (EditorApplication.isPlayingOrWillChangePlaymode && !_isPlayering)
            {
                _isPlayering = true;
                StartPlaying();
                _startLayoutBuildFlag = true;
            }
            else if (!EditorApplication.isPlaying && _isPlayering)
            {
                _isPlayering = false;
                EndPlaying();
                _startLayoutBuildFlag = true;
            }


            if (EditorApplication.isCompiling && !_isCompiling)
            {
                _isCompiling = true;
                StartCompile();
            }
            else if (!EditorApplication.isCompiling && _isCompiling)
            {
                _isCompiling = false;
                EndCompile();
                _startLayoutBuildFlag = true;
            }

            if (_startLayoutBuildFlag) 
            {
                _startLayoutBuildFlag = false;
                LayoutBuild();
            }

            if (_euiCore != null)
            {
                _euiCore.OnGUI();
//                Repaint();
            }
        }
    }
}