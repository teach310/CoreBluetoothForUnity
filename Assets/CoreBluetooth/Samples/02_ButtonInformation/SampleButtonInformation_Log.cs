using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleButtonInformation_Log : MonoBehaviour
    {
        [SerializeField] Text _logText;
        Stack<string> _logStack = new Stack<string>();

        public void AppendLog(int buttonID)
        {
            _logStack.Push($"{buttonIDToString(buttonID)}");
            if (_logStack.Count > 10) _logStack.Pop();
            _logText.text = string.Join(" ", _logStack.ToArray());
        }

        string buttonIDToString(int buttonID)
        {
            switch (buttonID)
            {
                case 1:
                    return "▲";
                case 2:
                    return "▶︎";
                case 3:
                    return "▼";
                case 4:
                    return "◀︎";
                case 5:
                    return "A";
                case 6:
                    return "B";
                default:
                    return buttonID.ToString();
            }
        }
    }
}