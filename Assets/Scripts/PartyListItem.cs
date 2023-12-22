using Photon.Realtime;
using UnityEngine;
using TMPro;

public class PartyListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    [HideInInspector] public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
