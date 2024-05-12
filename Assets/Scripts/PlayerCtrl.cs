using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl Instance;

    public Transform transformComp;

    private GameObject RroboCamCanvasGo;

    public PlayerStatsUiCtrl playerStatsUiCtrl;

    public Canvas playerHudCanvas;

    public PlayerRoboCamCtrl playerRoboCamCtrl;
    private float shieldRegenerationSpeed = 5f;

    private float _health = 100f;

    private float _shield = 100f;

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

    private void Start()
    {
        RroboCamCanvasGo = GameObject.FindWithTag("DroneCamCanvas");
        playerStatsUiCtrl.SetHealthInUi(_health);
        playerStatsUiCtrl.SetShieldInUi(_shield);
        SetRoboCamCanvas(false);
        playerHudCanvas.worldCamera = GameObject.FindObjectOfType<OVRCameraRig>().centerEyeAnchor.GetComponent<Camera>();
    }

    private void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            isRobotView = !isRobotView;
            playerRoboCamCtrl.SetRobotControlStatus(isRobotView);
            playerRoboCamCtrl.SetRobotToIdle();
            PlayerCtrl.Instance.SetRoboCamCanvas(isRobotView);
        }
    }

    public void SetRoboCamCanvas(bool isActive){
        RroboCamCanvasGo.SetActive(isActive);
    }

    public void ReduceHealthAndShieldOfPlayer(float m_reductionFactor)
    {
        _shield -= m_reductionFactor;
        StopAllCoroutines();
        StartCoroutine(StartUnHitTimerAfterHit());
        if (_shield < 0)
        {
            _shield = 0;
            _health -= m_reductionFactor;
            if(_health < 0)
            {
                _health = 0;
            }
            playerStatsUiCtrl.SetHealthInUi(_health);
            playerStatsUiCtrl.StartShieldDepletionEffect();
        }
        playerStatsUiCtrl.SetShieldInUi(_shield);
    }


    private IEnumerator StartUnHitTimerAfterHit()
    {
        StopCoroutine(StartUnHitTimerAfterHit());
        float timeOfUnhit = 4f;
        while (timeOfUnhit > 0)
        {
            yield return new WaitForSeconds(1f);
            timeOfUnhit -= 1f;
            if(timeOfUnhit <= 0f)
            {
                StopCoroutine(StartUnHitTimerAfterHit());
                StartCoroutine(IncreaseShieldSlowly());
            }
        }
    }


    private IEnumerator IncreaseShieldSlowly()
    {
        StopCoroutine(IncreaseShieldSlowly());
        while (_shield < 100f)
        {
            yield return new WaitForEndOfFrame();
            _shield += (Time.deltaTime * shieldRegenerationSpeed);
            if(_shield > 100f)
            {
                _shield = 100f;
                StopCoroutine(IncreaseShieldSlowly());
            }
            playerStatsUiCtrl.SetShieldInUi(_shield);
        }
    }

}
