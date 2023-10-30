using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleButtonInformation_Log : MonoBehaviour
    {
        [SerializeField] Text _logText;
        Queue<string> _logQueue = new Queue<string>();

        float remainigTime = 1f;
        bool isRemaining = false;
        float removeInterval = 0.1f;
        bool isRemoving = false;
        float timer = 0f;

        void Start()
        {
            _logText.text = string.Empty;
        }

        void Update()
        {
            if (_logQueue.Count == 0)
                return;

            timer += Time.deltaTime;
            if (isRemaining)
            {
                if (timer > remainigTime)
                {
                    isRemaining = false;
                    isRemoving = true;
                    timer = 0f;
                }
            }

            if (isRemoving)
            {
                if (timer > removeInterval)
                {
                    _logQueue.Dequeue();
                    timer = 0f;
                    UpdateView();
                    if (_logQueue.Count == 0)
                        isRemoving = false;
                }
            }
        }

        public void AppendLog(int buttonID, bool isPressed)
        {
            if (!isPressed)
                return;

            if (isRemoving)
            {
                _logQueue.Clear();
                isRemoving = false;
            }

            _logQueue.Enqueue(buttonIDToString(buttonID));
            if (_logQueue.Count > 20) _logQueue.Dequeue();

            timer = 0f;
            isRemaining = true;
            UpdateView();
        }

        void UpdateView()
        {
            _logText.text = string.Join(" ", _logQueue.ToArray());
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