using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Oculus.Interaction;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputField;

    public GameObject connectBtnGo;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        inputField.onValueChanged.AddListener(OnChangeInputValue);
    }

    public void OnChangeInputValue(string m_text)
    {
        if(inputField.text != null || inputField.text != string.Empty)
        {
            connectBtnGo.SetActive(true);
        }
        else
        {
            connectBtnGo.SetActive(false);
        }
    }

    public void ConnectWithName()
    {
        PhotonNetwork.NickName = inputField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectAnonymously()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("Connection to Server");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Can Matchmake now" + PhotonNetwork.NickName);
    }

    private void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(OnChangeInputValue);
    }
}
