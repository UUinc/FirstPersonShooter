using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] CustomToggle fpsToggle;
    [SerializeField] CustomToggle fullscreenToggle;
    [SerializeField] CustomSlider sensitivitySlider;
    [SerializeField] CustomSlider masterSlider;
    [SerializeField] CustomSlider musicSlider;
    [SerializeField] CustomSlider sfxSlider;

    //KEYS
    public static string FPS_KEY = "fps";
    public static string FULLSCREEN_KEY = "fullscreen";
    public static string SENSITIVITY_KEY = "sensitivity";

    private void Start()
    {
        //FPS
        fpsToggle.isOn = PlayerPrefs.GetInt(FPS_KEY, 0) == 1;
        fpsToggle.OnToggleChange();

        //Fullscreen
        fullscreenToggle.isOn = PlayerPrefs.GetInt(FULLSCREEN_KEY, 0) == 1;
        fullscreenToggle.OnToggleChange();

        //Sensitivity
        Debug.Log(PlayerPrefs.GetFloat(SENSITIVITY_KEY, 1f));
        sensitivitySlider.OnValueChange(PlayerPrefs.GetFloat(SENSITIVITY_KEY, 1f));

        //Sound Volume
        masterSlider.OnValueChange(AudioManager.Instance.Get_MasterVolume());
        musicSlider.OnValueChange(AudioManager.Instance.Get_MusicVolume());
        sfxSlider.OnValueChange(AudioManager.Instance.Get_SFXVolume());
    }

    public void SetFPS(bool value)
    {
        AudioManager.Instance.Play("Click");
        PlayerPrefs.SetInt(FPS_KEY, value ? 1 : 0);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        AudioManager.Instance.Play("Click");
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullScreen ? 1 : 0);
        Screen.fullScreen = isFullScreen;
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, value);
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.Set_MasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.Set_MusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.Instance.Set_SFXVolume(value);
    }
}
