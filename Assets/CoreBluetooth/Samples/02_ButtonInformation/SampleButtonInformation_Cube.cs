using UnityEngine;

namespace CoreBluetoothSample
{
    public class SampleButtonInformation_Cube : MonoBehaviour
    {
        Transform _transform;

        float _distance = 3f;
        Vector3 _posOrigin;

        void Start()
        {
            _transform = transform;
            _posOrigin = _transform.localPosition;
        }

        public void Action(int buttonId, bool isPressed)
        {
            if (buttonId < 5)
            {
                SetPos(buttonId, isPressed);
            }
            else
            {
                SetScale(buttonId, isPressed);
            }
        }

        void SetPos(int buttonId, bool isPressed)
        {
            if (!isPressed)
            {
                _transform.localPosition = _posOrigin;
                return;
            }

            Vector3 pos = _posOrigin;
            switch (buttonId)
            {
                case 1:
                    pos += Vector3.up * _distance;
                    break;
                case 2:
                    pos += Vector3.right * _distance;
                    break;
                case 3:
                    pos += Vector3.down * _distance;
                    break;
                case 4:
                    pos += Vector3.left * _distance;
                    break;
            }
            _transform.localPosition = pos;
        }

        void SetScale(int buttonId, bool isPressed)
        {
            if (!isPressed)
            {
                _transform.localScale = Vector3.one;
                return;
            }

            Vector3 scale = Vector3.one;
            switch (buttonId)
            {
                case 5:
                    scale *= 2f;
                    break;
                case 6:
                    scale *= 0.5f;
                    break;
            }
            _transform.localScale = scale;
        }
    }
}
