using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
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
        public int id;
        public string gameState;
    }

    [Serializable]
    public struct SaveData
    {
        public List<UserGameData> playersDataList;
    }

    [SerializeField, HideInInspector]
    private SaveData savedData;

    private string savePath = string.Empty;

    string saveDataJsonString = string.Empty;

    private int currentPlayerDataId = -1;

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
        savedData = new SaveData() { playersDataList = new List<UserGameData>() };
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        savePath = Application.persistentDataPath + @"\eliteHunterData.txt";
        GetSavedData();
    }

    public void SetNewPlayerData(string  m_playerName)
    {
        UserGameData userGameData = new UserGameData() {
            playerName = m_playerName,
            id = GetPlayerDataList().Count,
            playerSkin = "BLUE",
        };
        savedData.playersDataList.Add(userGameData);

        SetCurrentPlayerIndex(userGameData.id);
        SetSaveData();
    }

    public void SetSaveData()
    {
        saveDataJsonString = JsonUtility.ToJson(savedData);
        File.WriteAllText(savePath, saveDataJsonString);
    }

    private void GetSavedData()
    {
        if(!File.Exists(savePath))
        {
            return;
        }
        saveDataJsonString = File.ReadAllText(savePath);

        savedData = JsonUtility.FromJson<SaveData>(saveDataJsonString);
    } 

    public bool PlayerNameAlreadyExists(string m_name)
    {
        int playerIndex = -1;
        playerIndex = GetPlayerDataList().FindIndex(each => each.playerName == m_name);
        return playerIndex >= 0;
    }

    public UserGameData GetCurrentUserData()
    {
        return GetPlayerDataList().Find(each => each.id == currentPlayerDataId);
    }

    public void SetCurrentPlayerIndex(int id)
    {
        currentPlayerDataId = id;
    }

    private List<UserGameData> GetPlayerDataList()
    {
        if(savedData.playersDataList == null)
        {
            return new List<UserGameData>();
        }
        else
        {
            return savedData.playersDataList;
        }
    }


    public LoadGameProfilesListUiCtrl.UiData PrepareDataForLoadGameProfiles()
    {

        List<LoadGameProfileUiCtrl.UiData> uiData = new List<LoadGameProfileUiCtrl.UiData>();
        
            /*for (int i = GetPlayerDataList().Count - 1; i >= 0; i--)
            {
                LoadGameProfileUiCtrl.UiData data = new LoadGameProfileUiCtrl.UiData();
                data.name = savedData.playersDataList[i].playerName;
                data.id = savedData.playersDataList[i].id;
                uiData.Add(data);
            }*/

        return new LoadGameProfilesListUiCtrl.UiData() { LoadGameProfilesData = uiData };

    }

    private void OnSceneLoaded(Scene m_scene, LoadSceneMode m_loadSceneMode)
    {
        _sceneName = m_scene.name;
    }

    public void SaveDataOfCurrentUser()
    {
        /*UserGameData userGameData = GetCurrentUserData();
        userGameData.playerGameData = PlayerCtrl.LocalInstance.GetPlayerGameData();
        userGameData.machinesData = GameManager.Instance.GetMachinesData();
        userGameData.garbageDetails = GameManager.Instance.GetGarbageDetails();
        userGameData.gameState = GameManager.Instance.CurrentGameState.ToString();

        int index = GetPlayerDataList().FindIndex(each => each.id == currentPlayerDataId);
        savedData.playerDataList[index] = userGameData;
        SetSaveData();*/
    }
}
