using UnityEngine;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleLightControl_Central : MonoBehaviour
    {
        [SerializeField] SampleLightControl_RGBSliders _rgbSliders;
        [SerializeField] Image _colorImage;

        void Start()
        {
            _rgbSliders.OnColorChanged.AddListener((color) =>
            {
                _colorImage.color = color;
            });
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rgbSliders.SetColor(new Color32(
                    (byte)Random.Range(0, 255),
                    (byte)Random.Range(0, 255),
                    (byte)Random.Range(0, 255),
                    255
                ));
            }
        }
    }
}
