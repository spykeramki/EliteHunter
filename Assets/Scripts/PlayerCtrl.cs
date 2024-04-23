using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl Instance;

    public Transform transformComp;

    public PlayerRoboCamCtrl playerRoboCamCtrl;

    public GameObject RroboCamCanvasGo;

    private bool isRobotView = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
    }

    private void Update(){
        if(OVRInput.GetDown(OVRInput.Button.One)){
            isRobotView = !isRobotView;
            playerRoboCamCtrl.SetRobotControlStatus(isRobotView);
            playerRoboCamCtrl.SetRobotToIdle();
            SetRoboCamCanvas(isRobotView);
        }
    }

    private void SetRoboCamCanvas(bool isActive){
        RroboCamCanvasGo.SetActive(isActive);
    }

}
