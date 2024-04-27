using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldCtrl : MonoBehaviour
{
    public TMP_InputField inputField;

    public PointableUnityEventWrapper pointableUnityEventWrapper;

    private void Awake()
    {
        pointableUnityEventWrapper.WhenSelect.AddListener((PointerEvent) =>
        {
            inputField.Select();
        });
    }

    /*private void OnDisable()
    {
        pointableUnityEventWrapper.WhenSelect.RemoveListener((PointerEvent) =>
        {
            inputField.Select();
        });
    }*/
}
