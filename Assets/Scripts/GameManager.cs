using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject FPS;
    
    private void Start()
    {
        SetSettings();
        LockCursor(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(false);
            Destroy(RoomManager.Instance.gameObject);
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }
    }
    void SetSettings()
    {
        //FPS
        bool isFPS = PlayerPrefs.GetInt(SettingsManager.FPS_KEY, 0) == 1;
        FPS.SetActive(isFPS);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

}
