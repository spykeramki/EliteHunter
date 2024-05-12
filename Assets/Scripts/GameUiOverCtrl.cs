using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class GameUiOverCtrl : MonoBehaviour
{
    [Serializable]
    public struct UiData
    {
        public int score;
        public int time;
    }

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI timeText;

    public GameObject gameOverBtnsParentGo;

    public void SetDataInUI(UiData m_uiData)
    {
        scoreText.text = m_uiData.score.ToString();
        timeText.text = m_uiData.time.ToString();
        gameObject.SetActive(true);
        gameOverBtnsParentGo.SetActive(true);
        GameManager.Instance.mainUi.SetActive(true);
    }

    public void OnClickHomeBtn()
    {
        gameObject.SetActive(false);
        gameOverBtnsParentGo.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickQuitBtn()
    {
        gameObject.SetActive(false);
        gameOverBtnsParentGo.SetActive(false);
        Application.Quit();
    }
}
