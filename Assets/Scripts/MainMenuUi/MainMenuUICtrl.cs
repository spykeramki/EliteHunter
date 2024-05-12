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
using Photon.Pun;
using Photon.Realtime;

public class MainMenuUICtrl : MonoBehaviourPunCallbacks
{

    public LoadingScreenCtrl loadingScreenCtrl;

    public PlayerDetailsCtrl playerDetailsCtrl;

    public RoomDetailsCtrl roomDetailsCtrl;

    public JoinRoomCtrl joinRoomCtrl;

    [Serializable]
    public struct MainMenuUiGos
    {
        public GameObject home;
        public GameObject newPlayer;
        public GameObject quit;
        public GameObject instructions;
        public GameObject credits;
        public GameObject multiplayerHome;
        public GameObject createRoom;
        public GameObject joinRoom;
    }

    public MainMenuUiGos mainMenuUiGos;

    public GameObject mainMenuBtnsParentGo;
    public GameObject playBtnsParentGo;
    public GameObject backBtnGo;
    public GameObject yesAndNoBtnParentGo;
    public GameObject startBtnGo;
    public GameObject createBtnGo;

    public TMP_InputField inputField;

    public TouchScreenKeyboard overlayKeyboard;

    public Transform playerCamTransform;

    public TextMeshProUGUI mainMenuSupportText;

    private Stack<List<GameObject>> backFunctionQueue;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 800, 150, 50), "Continue"))
        {
            OnClickStartBtn();
        }

        if (GUI.Button(new Rect(160, 800, 150, 50), "Play"))
        {
            OnClickPlayBtn();
        }
        if (GUI.Button(new Rect(320, 800, 150, 50), "MultiPlayer"))
        {
            OnClickMultiPlayerBtnBtn();
        }
        if (GUI.Button(new Rect(480, 800, 150, 50), "JoinRoom"))
        {
            OnClickJoinRoomBtn();
        }
        if (GUI.Button(new Rect(320, 1000, 150, 50), "Refresh List"))
        {
            OnClickRefreshBtn();
        }
    }

    private void Start()
    {
        backFunctionQueue = new Stack<List<GameObject>>();
        SetInitialUi(DataManager.Instance.IsNewPlayer);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            transform.position = playerCamTransform.position - new Vector3(0F, 1.5F, 0F);

            transform.rotation = playerCamTransform.rotation;
            transform.up = Vector3.up;
            //transform.rotation *= Quaternion.FromToRotation(transform.up, Vector3.up);
        }
        if(overlayKeyboard != null)
        {
            inputField.text = overlayKeyboard.text;
        }
    }

    #region MAIN MENU
    private void SetInitialUi(bool m_isNewPlayer)
    {
        List<GameObject> playerNameUiGos = new List<GameObject>
        {
            mainMenuUiGos.newPlayer
        };
        List<GameObject> mainMenuUi = new List<GameObject>
        {
            mainMenuUiGos.home,
            mainMenuBtnsParentGo
        };
        SetActivenessOfGos(playerNameUiGos, m_isNewPlayer);
        SetActivenessOfGos(mainMenuUi, false);
        if (!m_isNewPlayer)
        {
            SetSupportTextWithPlayerName(DataManager.Instance.GetUserData().playerName);
            ConnectWithName(playerDetailsCtrl.PlayerName);
        }
    }

    public void OnClickPlayBtn()
    {
        HideMainMenuUiAndPushToStack();

        List<GameObject> objectToActive = new List<GameObject>
        {
            mainMenuUiGos.home,
            backBtnGo,
            playBtnsParentGo
        };
        backFunctionQueue.Push(objectToActive);
        StartCoroutine(SetActivenessOfGosAfterATime(objectToActive, true));
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

    public void OnClickMultiPlayerBtnBtn()
    {
        DataManager.Instance.JoinLobby();
        List<GameObject> objectToBeInactive = new List<GameObject>
        {
            mainMenuUiGos.home,
            backBtnGo,
            playBtnsParentGo
        };
        backFunctionQueue.Push(objectToBeInactive);
        SetActivenessOfGos(objectToBeInactive, false);

        List<GameObject> objectToActive = new List<GameObject>
        {
            mainMenuUiGos.multiplayerHome,
            backBtnGo
        };
        backFunctionQueue.Push(objectToActive);
        StartCoroutine(SetActivenessOfGosAfterATime(objectToActive, true));
    }

    public void OnClickCreateRoomBtn()
    {
        List<GameObject> objectToBeInactive = new List<GameObject>
        {
            mainMenuUiGos.multiplayerHome,
            backBtnGo
        };
        backFunctionQueue.Push(objectToBeInactive);
        SetActivenessOfGos(objectToBeInactive, false);

        List<GameObject> objectToActive = new List<GameObject>
        {
            mainMenuUiGos.createRoom,
            backBtnGo
        };
        backFunctionQueue.Push(objectToActive);
        StartCoroutine(SetActivenessOfGosAfterATime(objectToActive, true));
    }

    public void OnClickCreateBtn()
    {
        List<GameObject> objectToBeInactive = new List<GameObject>
        {
            mainMenuUiGos.createRoom,
            backBtnGo,
            createBtnGo,
        };
        SetActivenessOfGos(objectToBeInactive, false);
        CreateRoom(roomDetailsCtrl.RoomName);
    }

    public void OnClickJoinRoomBtn()
    {
        List<GameObject> objectToBeInactive = new List<GameObject>
        {
            mainMenuUiGos.multiplayerHome,
            backBtnGo
        };
        backFunctionQueue.Push(objectToBeInactive);
        SetActivenessOfGos(objectToBeInactive, false);

        OnClickRefreshBtn();

        List<GameObject> objectToActive = new List<GameObject>
        {
            mainMenuUiGos.joinRoom,
            backBtnGo
        };
        backFunctionQueue.Push(objectToActive);
        StartCoroutine(SetActivenessOfGosAfterATime(objectToActive, true));
        
    }

    public void OnClickRefreshBtn()
    {
        List<RoomInfo> roomInfos = new List<RoomInfo>();
        foreach (KeyValuePair<string, RoomInfo> item in DataManager.Instance.GetRoomsData())
        {
            roomInfos.Add(item.Value);
        }
        joinRoomCtrl.CreateAndSetJoinRoomItem(roomInfos);
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
        DataManager.Instance.SetNewPlayerData(playerDetailsCtrl.PlayerName, playerDetailsCtrl.PlayerSkin);
        ConnectWithName(playerDetailsCtrl.PlayerName);
        List<GameObject> objectsToHide = new List<GameObject>
        {
            mainMenuUiGos.newPlayer,
            startBtnGo
        };
        SetActivenessOfGos(objectsToHide, false);
    }

    public void ShowMainMenu()
    {
        List<GameObject> playerNameUiGos = new List<GameObject>
        {
            mainMenuUiGos.newPlayer,
            startBtnGo
        };
        List<GameObject> mainMenuUi = new List<GameObject>
        {
            mainMenuUiGos.home,
            mainMenuBtnsParentGo
        };
        SetSupportTextWithPlayerName(DataManager.Instance.GetUserData().playerName);
        SetActivenessOfGos(playerNameUiGos, false);
        StartCoroutine(SetActivenessOfGosAfterATime(mainMenuUi, true));
    }

    public void SetSupportTextWithPlayerName(string m_name)
    {
        mainMenuSupportText.text = "Welcome to the world of hunters " + m_name;

    }

    public void StartGame()
    {
        List<GameObject> objectToHide = new List<GameObject>
        {
            backBtnGo,
            mainMenuUiGos.newPlayer,
            mainMenuBtnsParentGo
        };
        SetActivenessOfGos(objectToHide, false);

        loadingScreenCtrl.ShowLoadingScreen(Constants.GAME_SCENE);
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

    #endregion

    #region MULTIPLAYER RELATED

    public void ConnectWithName(string m_name)
    {
        PhotonNetwork.NickName = m_name;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        ShowMainMenu();
    }


    private void CreateRoom(string m_roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(m_roomName, roomOptions, DataManager.Instance.CustomLobby);
    }

    public override void OnCreatedRoom()
    {
        Debug.LogErrorFormat("Room Created " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
        OnClickBackBtn();
    }
    #endregion

}
