using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Oculus.Interaction;
using System;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public Action ConnectedToServer;

    public void ConnectWithName(string m_name)
    {
        PhotonNetwork.NickName = m_name;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("Connection to Server");
    }

    public override void OnConnectedToMaster()
    {
        ConnectedToServer?.Invoke();
    }
}
