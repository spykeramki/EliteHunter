using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomDetailsCtrl : MonoBehaviour
{
    public TextMeshProUGUI nameInstruction;

    public MainMenuUICtrl mainMenuUICtrl;

    private string TYPE_INSTRUCTION = "Good to go";

    private string NAME_ALREADY_EXISTS = "Room Already Exists";

    private string EMPTY_TEXT_WARNING = "Room Name cannot be empty";

    private string _roomName;

    public string RoomName
    {
        get { return _roomName; }
    }

    private bool _acceptRoomName;
    public bool AcceptRoomName
    {
        get { return _acceptRoomName; }
    }

    public void OnChangeName(string m_name)
    {
        _roomName = m_name;
        bool doesRoomNameExist = DataManager.Instance.IsRoomAlreadyExists(m_name);
        if (m_name == string.Empty)
        {
            nameInstruction.text = EMPTY_TEXT_WARNING;
            errorSetings();
            return;
        }
        if (doesRoomNameExist)
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
        _acceptRoomName = true;
        mainMenuUICtrl.createBtnGo.SetActive(true);
    }

    private void errorSetings()
    {
        nameInstruction.color = Color.red;
        _acceptRoomName = false;
        mainMenuUICtrl.createBtnGo.SetActive(false);
    }

}
