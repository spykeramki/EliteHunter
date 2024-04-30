using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuUICtrl : MonoBehaviour
{

    public LoadingScreenCtrl loadingScreenCtrl;

    public PlayerDetailsCtrl playerDetailsCtrl;

    [Serializable]
    public struct MainMenuUiGos
    {
        public GameObject home;
        public GameObject newPlayer;
        public GameObject quit;
        public GameObject instructions;
        public GameObject credits;
    }

    public MainMenuUiGos mainMenuUiGos;

    public GameObject mainMenuBtnsParentGo;
    public GameObject backBtnGo;
    public GameObject yesAndNoBtnParentGo;
    public GameObject startBtnGo;

    public TMP_InputField inputField;

    public TouchScreenKeyboard overlayKeyboard;

    public Transform playerTransform;

    private Stack<List<GameObject>> backFunctionQueue;


    private void Start()
    {
        backFunctionQueue = new Stack<List<GameObject>>();
        /*pointableUnityEventWrapper.WhenSelect.AddListener((PointerEvent) =>
        {
            inputField.Select();
        });*/
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            transform.position = playerTransform.position - new Vector3(0F, 1.5F, 0F);
            transform.forward = playerTransform.forward;
        }
        if(overlayKeyboard != null)
        {
            inputField.text = overlayKeyboard.text;
        }
    }

    public void OnClickPlayBtn()
    {

        OnClickPlayOption(ref mainMenuUiGos.newPlayer);
    }

    public void OnClickQuitBtn()
    {
        OnClickOfQuitOption(ref mainMenuUiGos.quit);
    }

    public void OnClickInstructionsBtn()
    {
        OnClickAMainMenuOption(ref mainMenuUiGos.instructions);
    }

    public void OnClickCreditsBtn()
    {
        OnClickAMainMenuOption(ref mainMenuUiGos.credits);
    }

    public void OnClickBackBtn()
    {
        List<GameObject> gosToDeactivate = backFunctionQueue.Pop();
        SetActivenessOfGos(gosToDeactivate, false);

        List<GameObject> gosToActivate = backFunctionQueue.Pop();
        StartCoroutine(SetActivenessOfGosAfterATime(gosToActivate, true));
    }

    private void OnClickAMainMenuOption(ref GameObject m_goToActive)
    {
        HideMainMenuUiAndPushToStack();

        List<GameObject> objectToActive = new List<GameObject>
        {
            m_goToActive,
            backBtnGo
        };
        backFunctionQueue.Push(objectToActive);
        StartCoroutine(SetActivenessOfGosAfterATime(objectToActive, true));
    }

    private void OnClickPlayOption(ref GameObject m_goToActive)
    {
        HideMainMenuUiAndPushToStack();

        List<GameObject> objectToActive = new List<GameObject>
        {
            m_goToActive,
            backBtnGo,
            startBtnGo
        };
        backFunctionQueue.Push(objectToActive);
        StartCoroutine(SetActivenessOfGosAfterATime(objectToActive, true));
    }

    private void OnClickOfQuitOption(ref GameObject m_goToActive)
    {
        HideMainMenuUiAndPushToStack();

        List<GameObject> objectToActive = new List<GameObject>
        {
            m_goToActive,
            backBtnGo,
            yesAndNoBtnParentGo
        };
        backFunctionQueue.Push(objectToActive);
        StartCoroutine(SetActivenessOfGosAfterATime(objectToActive, true));
    }

    private void HideMainMenuUiAndPushToStack()
    {
        List<GameObject> objectForBackFumctionality = new List<GameObject>
        {
            mainMenuUiGos.home,
            mainMenuBtnsParentGo
        };
        backFunctionQueue.Push(objectForBackFumctionality);
        SetActivenessOfGos(objectForBackFumctionality, false);
    }

    public void OnClickStartBtn()
    {
        if(playerDetailsCtrl.AcceptPlayerName){
            startBtnGo.SetActive(false);
            backBtnGo.SetActive(false);
            DataManager.Instance.SetNewPlayerData(playerDetailsCtrl.PlayerName);
            loadingScreenCtrl.ShowLoadingScreen("01Main");
        }
    }

    private void OnClickMultiPlayerBtnBtn()
    {
        /*LoadGameProfilesListUiCtrl.UiData loadProfilesUiData = DataManager.Instance.PrepareDataForLoadGameProfiles();

        loadGameProfilesUiCtrl.SetDataInUi(loadProfilesUiData);
        loadGameProfilesUiCtrl.gameObject.SetActive(true);*/
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SetActivenessOfGos(List<GameObject> m_gos, bool isActive)
    {
        foreach (GameObject go in m_gos)
        {
            go.SetActive(isActive);
        }
    }

    private IEnumerator SetActivenessOfGosAfterATime(List<GameObject> m_gos, bool isActive)
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject go in m_gos)
        {
            go.SetActive(isActive);
        }
    }

    public void OnClickUiInputField()
    {
        Debug.Log("clicked input field");
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    public void OnDeselectInputField()
    {
        overlayKeyboard = null;
    }



}
