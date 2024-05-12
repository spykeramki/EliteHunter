using Photon.Pun;
using System;
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

    private float _timeSpent = 0f;
    public float TimeSpent
    {
        get { return _timeSpent; }
    }

    private int _score = 0;
    public int Score
    {
        get { return _score; }
    }

    public Renderer playerMeshRenderer;

    [Serializable]
    public struct PlayerMats
    {
        public Material blueSkin;
        public Material greenSkin;
        public Material redSkin;
    }

    public PlayerMats playerMats;

    public PhotonView photonView;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        RroboCamCanvasGo = GameObject.FindWithTag("DroneCamCanvas");
        playerStatsUiCtrl.SetHealthInUi(_health);
        playerStatsUiCtrl.SetShieldInUi(_shield);
        SetRoboCamCanvas(false);
        playerHudCanvas.worldCamera = GameObject.FindObjectOfType<OVRCameraRig>().centerEyeAnchor.GetComponent<Camera>();

        playerHudCanvas.gameObject.SetActive(photonView.IsMine);

        playerRoboCamCtrl.EnableOrDisableCamers(photonView.IsMine);
    }

    private void Update()
    {

        if (GameManager.Instance.IsGameStartedAndNotEnded())
        {
            if(OVRInput.GetDown(OVRInput.Button.One))
            {
                isRobotView = !isRobotView;
                playerRoboCamCtrl.SetRobotControlStatus(isRobotView);
                playerRoboCamCtrl.SetRobotToIdle();
                SetRoboCamCanvas(isRobotView);
            }
            _timeSpent += Time.deltaTime;
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
                GameManager.Instance.GameOver();
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

    public void SetMaterialOnPlayerChoice(string m_skin)
    {
        photonView.RPC("SetMaterialOnPlayerChoiceRPC", RpcTarget.All, m_skin);
    }

    [PunRPC]
    public void SetMaterialOnPlayerChoiceRPC(string m_skin)
    {
        Material[] playerMat = playerMeshRenderer.materials;
        switch (m_skin)
        {
            case Constants.BLUE_SKIN:
                playerMat[0] = playerMats.blueSkin;
                break;
            case Constants.GREEN_SKIN:
                playerMat[0] = playerMats.greenSkin;
                break;
            case Constants.RED_SKIN:
                playerMat[0] = playerMats.redSkin;
                break;
        }
        playerMeshRenderer.materials = playerMat;
    }

    public void IncreaseScoreByOne()
    {
        _score++;
    }
}
