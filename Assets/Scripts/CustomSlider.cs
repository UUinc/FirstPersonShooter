using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

public class CustomSlider : MonoBehaviour
{
    [System.Serializable]
    public class MyFunctionEvent : UnityEvent<float> { }
    public MyFunctionEvent onValueChange;

    private TMP_InputField inputField;
    private Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        inputField = GetComponentInChildren<TMP_InputField>();

        inputField.onValueChanged.AddListener((value) => OnTextValueChanged(value));
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value));
    }

    void OnTextValueChanged(string value)
    {
        bool parsed = float.TryParse(value, out float _value);
        if (!parsed) return;

        if (_value < slider.minValue || _value > slider.maxValue)
        {
            _value = _value < slider.minValue ? slider.minValue : slider.maxValue;
            inputField.text = _value.ToString();
        }

        OnValueChange(slider.value);
    }

    void OnSliderValueChanged(float value)
    {

        OnValueChange(value);
    }

    public void OnValueChange(float value)
    {
        slider.value = value;
        inputField.text = value.ToString("0.00");

        onValueChange.Invoke(value);
    }
}
