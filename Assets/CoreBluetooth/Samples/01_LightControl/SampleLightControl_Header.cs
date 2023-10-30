using UnityEngine;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleLightControl_Header : MonoBehaviour
    {
        [SerializeField] Text stateLabel;

        public void SetStateText(string text)
        {
            stateLabel.text = text;
        }
    }
}
