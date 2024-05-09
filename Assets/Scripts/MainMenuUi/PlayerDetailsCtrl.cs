using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDetailsCtrl : MonoBehaviour
{
    public TextMeshProUGUI nameInstruction;

    public ToggleGroup toggleGroup;

    public MainMenuUICtrl mainMenuUICtrl;

    private string TYPE_INSTRUCTION = "Good to go";

    private string NAME_ALREADY_EXISTS = "Hey! You have a twin with same name.\ngive another name";

    private string EMPTY_TEXT_WARNING = "You may feel Empty.\nBut you are more than nothing";

    private string _playerName;

    private string _playerSkin;
    public string PlayerName {
        get{ return _playerName; }
    }
    public string PlayerSkin
    {
        get { return _playerSkin; }
    }

    private bool _acceptPlayerName;
    public bool AcceptPlayerName {
        get{ return _acceptPlayerName;}
    }

    public void OnChangeName(string m_name)
    {
        _playerName = m_name;
        bool doesPlayerNameExist = false;//DataManager.Instance.PlayerNameAlreadyExists(m_name);
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

    public void OnChangeSkin(bool m_skinSelected)
    {
        if (m_skinSelected)
        {
            foreach (Toggle toggle in toggleGroup.ActiveToggles())
            {
                if (toggle.isOn)
                {
                    _playerSkin = toggle.name;
                }
            }
        }
    }

    private void goodToGoSettings()
    {
        nameInstruction.color = Color.green;
        _acceptPlayerName = true;
        mainMenuUICtrl.startBtnGo.SetActive(true);
    }

    private void errorSetings()
    {
        nameInstruction.color = Color.red;
        _acceptPlayerName = false;
        mainMenuUICtrl.startBtnGo.SetActive(false);
    }

}
