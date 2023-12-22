using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private float lastTime;
    private int frames;
    private TextMeshProUGUI fpsText;

    private void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        frames++;
        float time = Time.realtimeSinceStartup;
        if (time - lastTime >= 1.0f)
        {
            float fps = frames / (time - lastTime);
            frames = 0;
            lastTime = time;
            fpsText.text = fps.ToString("F2");
        }
    }
}
