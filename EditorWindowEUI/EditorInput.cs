using System.Collections.Generic;
using UnityEngine;

namespace EditorWindowEUI
{
    public class EditorInput
    {
        private Dictionary<KeyCode, int> _keyState = new Dictionary<KeyCode, int>();

        private Dictionary<int, int> _buttonState = new Dictionary<int, int>();

        public const int None = 0;
        public const int KeyDown = 1;
        public const int KeyPress = 3;

        public const int LeftButton = 0;
        public const int RightButton = 1;

        public void SetMouseState(int button, int state)
        {
            if (_buttonState.ContainsKey(button))
            {
                _buttonState[button] = state;
            }
            else
            {
                _buttonState.Add(button, state);
            }
        }

        public bool GetMouse(int button)
        {
            if (_buttonState.ContainsKey(button))
            {
                return _buttonState[button] == KeyPress;
            }

            return false;
        }

        public bool GetMouseDown(int button)
        {
            if (_buttonState.ContainsKey(button))
            {
                return _buttonState[button] == KeyDown;
            }

            return false;
        }

  

        public void SetKeyState(KeyCode k, int state)
        {
            if (_keyState.ContainsKey(k))
            {
                _keyState[k] = state;
            }
            else
            {
                _keyState.Add(k, state);
            }
        }


        public bool GetKey(KeyCode k)
        {
            if (_keyState.ContainsKey(k))
            {
                return _keyState[k] == KeyPress;
            }

            return false;
        }

        public int GetKeyState(KeyCode k)
        {
            if (_keyState.ContainsKey(k))
            {
                return _keyState[k];
            }

            return None;
        }

        public bool GetKeyDown(KeyCode k)
        {
            if (_keyState.ContainsKey(k))
            {
                return _keyState[k] == KeyDown;
            }

            return false;
        }

      
    }
}