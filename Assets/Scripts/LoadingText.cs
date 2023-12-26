using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
{
    TMP_Text loading;
    const float step = 0.5f;
    float timer = step;
    float count = 0;
    void Start()
    {
        loading = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if(timer <= 0)
        {
            timer = step;
            count++;
            if (count == 4) count = 0;

            switch(count)
            {
                case 0:
                    loading.text = "loading";
                    break;
                case 1:
                    loading.text = "loading.";
                    break;
                case 2:
                    loading.text = "loading..";
                    break;
                case 3:
                    loading.text = "loading...";
                    break;

            }
        }

        timer -= Time.deltaTime;
    }
}
