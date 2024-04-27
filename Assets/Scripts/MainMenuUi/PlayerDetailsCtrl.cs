using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDetailsCtrl : MonoBehaviour
{
    public TextMeshProUGUI nameInstruction;

    public Button startBtn;

    public LoadingScreenCtrl loadingScreenCtrl;

    private string TYPE_INSTRUCTION = "Good to go";

    private string NAME_ALREADY_EXISTS = "Hey! You have a twin with same name.\ngive another name";

    private string EMPTY_TEXT_WARNING = "You may feel Empty.\nBut you are more than nothing";

    private string _playerName;

    private void Start()
    {
        _playerName = string.Empty;
        startBtn.onClick.AddListener(OnClickStartBtn);
    }

    public void OnChangeName(string m_name)
    {
        _playerName = m_name;
        bool doesPlayerNameExist =  DataManager.Instance.PlayerNameAlreadyExists(m_name);
        if(m_name == string.Empty)
        {
            nameInstruction.text = EMPTY_TEXT_WARNING;
            errorSetings();
            return;
        }
        if (doesPlayerNameExist)
        {
            nameInstruction.text = NAME_ALREADY_EXISTS;
            errorSetings();
        }
        else
        {
            nameInstruction.text = TYPE_INSTRUCTION;
            goodToGoSettings();

        }
    }

    private void goodToGoSettings()
    {
        nameInstruction.color = Color.green;
        startBtn.interactable = true;
    }

    private void errorSetings()
    {
        nameInstruction.color = Color.red;
        startBtn.interactable = false;
    }

    public void OnClickStartBtn()
    {
        DataManager.Instance.SetNewPlayerData(_playerName);
        loadingScreenCtrl.ShowLoadingScreen("01Main");
    }

}
