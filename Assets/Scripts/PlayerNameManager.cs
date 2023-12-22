using TMPro;
using Photon.Pun;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;

    const string usernameKey = "username";

    private void Start()
    {
        if (PlayerPrefs.HasKey(usernameKey))
        {
            string username = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = usernameInput.text = username;
        }
        else
        {
            PhotonNetwork.NickName = usernameInput.text = "Player " + Random.Range(0, 1000).ToString("000");
        }
    }
    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString(usernameKey, usernameInput.text);
    }
}
