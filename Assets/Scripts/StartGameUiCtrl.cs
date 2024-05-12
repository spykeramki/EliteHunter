using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameUiCtrl : MonoBehaviour
{
    public GameObject startBtn;

    public void SetDataInUI()
    {
        gameObject.SetActive(true);
        startBtn.SetActive(true);
    }

    public void OnClickStartBtn()
    {
        GameManager.Instance.StartGameOnMasterClient();
        startBtn.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.mainUi.SetActive(false);
    }
}
