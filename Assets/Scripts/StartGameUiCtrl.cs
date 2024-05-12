using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGameUiCtrl : MonoBehaviour
{
    public GameObject startBtn;
    public TextMeshProUGUI supportText;

    public void SetDataInUI(bool isSinglePlayer)
    {
        if (isSinglePlayer)
        {
            supportText.text = "Lets get you trained well";
        }
        else
        {
            supportText.text = "Wait for players before \nstarting the match";
        }
        gameObject.SetActive(true);
        startBtn.SetActive(true);
    }

    public void OnClickStartBtn()
    {
        GameManager.Instance.SetGameStarted(true);
        GameManager.Instance.StartGameOnMasterClient();
        startBtn.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.mainUi.SetActive(false);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void SetActivenessOfWaitingText()
    {

    }
}
