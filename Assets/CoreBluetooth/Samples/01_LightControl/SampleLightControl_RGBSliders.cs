using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleLightControl_RGBSliders : MonoBehaviour
    {
        [SerializeField] Slider _redSlider;
        [SerializeField] Text _redLabel;

        [SerializeField] Slider _greenSlider;
        [SerializeField] Text _greenLabel;

        [SerializeField] Slider _blueSlider;
        [SerializeField] Text _blueLabel;

        Color32 _color = Color.white;
        public Color32 Value => _color;

        UnityEvent<Color32> _onColorChanged = new UnityEvent<Color32>();
        public UnityEvent<Color32> OnColorChanged => _onColorChanged;
        bool _enableSliderValueChangedEvent = true;

        void Reset()
        {
            _redSlider = transform.Find("R/Slider").GetComponent<Slider>();
            _redLabel = transform.Find("R/Label").GetComponent<Text>();
            _greenSlider = transform.Find("G/Slider").GetComponent<Slider>();
            _greenLabel = transform.Find("G/Label").GetComponent<Text>();
            _blueSlider = transform.Find("B/Slider").GetComponent<Slider>();
            _blueLabel = transform.Find("B/Label").GetComponent<Text>();
        }

        void Start()
        {
            _redSlider.onValueChanged.AddListener((value) => OnSliderValueChanged());
            _greenSlider.onValueChanged.AddListener((value) => OnSliderValueChanged());
            _blueSlider.onValueChanged.AddListener((value) => OnSliderValueChanged());
        }

        public void SetColor(Color32 color)
        {
            _enableSliderValueChangedEvent = false;
            Color floatColor = color;
            _redSlider.value = floatColor.r;
            _greenSlider.value = floatColor.g;
            _blueSlider.value = floatColor.b;
            _enableSliderValueChangedEvent = true;
            OnSliderValueChanged();
        }

        void OnSliderValueChanged()
        {
            if (!_enableSliderValueChangedEvent)
                return;
            _color = new Color(_redSlider.value, _greenSlider.value, _blueSlider.value);
            _onColorChanged.Invoke(_color);

            _redLabel.text = $"{_color.r}";
            _greenLabel.text = $"{_color.g}";
            _blueLabel.text = $"{_color.b}";
        }
    }
}