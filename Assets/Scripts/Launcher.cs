using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField partyNameInputField;
    [SerializeField] TMP_Text partyNameText;
    [SerializeField] TMP_Text errorText;

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("main");
        Debug.Log("Joined Lobby");
    }

    public void CreateParty()
    {
        if (string.IsNullOrEmpty(partyNameInputField.text)) return;
        PhotonNetwork.CreateRoom(partyNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        partyNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.Instance.OpenMenu("room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Party Creation Failed: "+ message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("main");
    }
}
