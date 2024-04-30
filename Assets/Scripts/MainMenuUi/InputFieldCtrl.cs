using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldCtrl : MonoBehaviour
{
    private TMP_InputField m_InputField;
    private TouchScreenKeyboard m_Keyboard;
    private string _inputText = string.Empty;

    private void Awake()
    {
        m_InputField = GetComponent<TMP_InputField>();
    }


    private void Update()
    {
        if(m_Keyboard != null)
        {
            m_InputField.text = m_Keyboard.text;
        }
    }

    public void OnApplicationFocus()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus){
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(!pauseStatus){
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
