using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
    public bool isOn = true;
    [System.Serializable]
    public class MyFunctionEvent : UnityEvent<bool> { }
    public MyFunctionEvent onToggleChange;

    private Transform leftButton;
    private Transform rightButton;


    private void Start()
    {
        leftButton = transform.GetChild(0);
        rightButton = transform.GetChild(1);

        SetToggleColor();

        leftButton.GetComponent<Button>().onClick.AddListener(() => onLeftButtonClick());
        rightButton.GetComponent<Button>().onClick.AddListener(() => onRightButtonClick());
    }
    
    void SetOn(bool value)
    {
        isOn = value;
    }

    void SetToggleColor()
    {
        leftButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, isOn ? 0.23f : 0f);
        rightButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, isOn ? 0f : 0.23f);
    }

    void onLeftButtonClick()
    {
        SetOn(true);

        //Inovke custom toggle
        OnToggleChange();
    }

    void onRightButtonClick()
    {
        SetOn(false);

        //Inovke custom toggle
        OnToggleChange();
    }

    public void OnToggleChange()
    {
        SetToggleColor();
        onToggleChange.Invoke(isOn);
    }
}
