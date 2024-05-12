using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomCtrl : MonoBehaviour
{
    public JoinRoomItemCtrl joinRoomItemCtrlPrefab;

    public Transform joinRoomItemsParent;

    private List<JoinRoomItemCtrl> joinRoomItemsList = new List<JoinRoomItemCtrl>();

    public void CreateAndSetJoinRoomItem(List<RoomInfo> m_roomInfoList)
    {
        ResetUi();
        foreach (RoomInfo roomInfo in m_roomInfoList)
        {
            JoinRoomItemCtrl item = Instantiate(joinRoomItemCtrlPrefab, joinRoomItemsParent);
            item.SetDataInUi(roomInfo);
            joinRoomItemsList.Add(item);
        }
    }

    private void ResetUi()
    {
        if(joinRoomItemsList.Count > 0)
        {
            foreach (JoinRoomItemCtrl item in joinRoomItemsList)
            {
                Destroy(item.gameObject);
            }
        }
        joinRoomItemsList = new List<JoinRoomItemCtrl> ();
    }
}
