using UnityEngine;

namespace CoreBluetoothSample
{
    public class SampleLightControl_Peripheral : MonoBehaviour
    {
        [SerializeField] Light _light = null;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _light.color = new Color(
                    Random.Range(0, 255) / 255f,
                    Random.Range(0, 255) / 255f,
                    Random.Range(0, 255) / 255f
                );
            }
        }
    }
}
