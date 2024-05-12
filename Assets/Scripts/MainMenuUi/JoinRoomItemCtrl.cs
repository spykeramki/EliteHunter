using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomItemCtrl : MonoBehaviour
{
    public TextMeshProUGUI roomName;

    private RoomInfo roomInfo;

    public void SetDataInUi(RoomInfo m_roomInfo)
    {
        roomName.text = m_roomInfo.Name;
        roomInfo = m_roomInfo;
    }

    public void OnClickRoomBtn()
    {
        DataManager.Instance.JoinRoom(roomInfo);
    }

}
