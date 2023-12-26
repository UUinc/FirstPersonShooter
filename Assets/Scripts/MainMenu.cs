using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject UIBlur;
    private void OnEnable()
    {
        UIBlur.SetActive(false);
    }
    private void OnDisable()
    {
        UIBlur.SetActive(true);
    }
}
