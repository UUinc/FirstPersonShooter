using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
        AudioManager.Instance.Play("Click");
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
