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
}
