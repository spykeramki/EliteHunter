using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class DataManager : MonoBehaviourPunCallbacks
{
    public static DataManager Instance;

    private string _sceneName;

    [Serializable]
    public struct PlayerGameData
    {
        public string playerSkin;
    }

    [Serializable]
    public struct UserGameData
    {
        public string playerName;
        public string playerSkin;
    }

    [Serializable]
    public struct SavedData
    {
        public UserGameData playerData;
    }

    [SerializeField, HideInInspector]
    private SavedData saveData;

    private string savePath = string.Empty;

    string saveDataJsonString = string.Empty;

    private bool _isNewPlayer = false;
    public bool IsNewPlayer
    {
        get { return _isNewPlayer; }
    }


    #region MULTIPLAYER
    private TypedLobby customLobby = new TypedLobby("eliteHunterLobby", LobbyType.Default);
    public TypedLobby CustomLobby
    {
        get { return customLobby; }
    }

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    private GameObject spawnedPlayerModel;

    #endregion

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        savePath = Application.persistentDataPath + @"\eliteHunterData3.txt";
        CheckSavedData();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void SetNewPlayerData(string  m_playerName, string m_skin)
    {
        UserGameData userGameData = new UserGameData()
        {
            playerName = m_playerName,
            playerSkin = m_skin,
        };

        saveData.playerData = userGameData;

        SetSaveData();
    }

    public void SetSaveData()
    {
        saveDataJsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, saveDataJsonString);
    }

    private void CheckSavedData()
    {
        if (!File.Exists(savePath))
        {
            _isNewPlayer = true;
        }
        else
        {
            _isNewPlayer = false;
            saveDataJsonString = File.ReadAllText(savePath);

            saveData = JsonUtility.FromJson<SavedData>(saveDataJsonString);
        }
    } 

    public UserGameData GetUserData()
    {
        return saveData.playerData;
    }

    public Dictionary<string, RoomInfo> GetRoomsData()
    {
        return cachedRoomList;
    }

    public bool IsRoomAlreadyExists(string m_room)
    {
        return cachedRoomList.ContainsKey(m_room);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == Constants.GAME_SCENE)
        {
            spawnedPlayerModel = PhotonNetwork.Instantiate(Constants.PLAYER_PREFAB_NAME, Vector3.zero, Quaternion.identity);
        }
    }


    #region
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(customLobby);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby: " + PhotonNetwork.CurrentLobby.Name);
        cachedRoomList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }


    public void JoinRoom(RoomInfo m_roomInfo)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRoom(m_roomInfo.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(Constants.GAME_SCENE);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel(Constants.START_MENU_SCENE);
        PhotonNetwork.Destroy(spawnedPlayerModel);
    }
    #endregion
}
