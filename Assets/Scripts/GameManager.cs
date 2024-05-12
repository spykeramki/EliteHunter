using IngameDebugConsole;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public static UnityEvent Fire = new UnityEvent();
    public static UnityEvent SecondaryFire = new UnityEvent();

    public HandGrabInteractor rightHandGrabInteractor;
    public HandGrabInteractor leftHandGrabInteractor;

    public NavMeshSurface navMeshSurface;

    public EffectMesh robotViewEffectMesh;
    public Transform robotMeshVieTransform;

    public SpawnerCtrl cubeSpawnerCtrl;

    public GameUiOverCtrl gameUiOverCtrl;

    public StartGameUiCtrl startGameUiCtrl;

    public GameObject mainUi;

    private int _currentGameLevel = 0;

    private float _timeSpent = 0f;

    private int _score= 0;

    private int _maxLevel = 3;

    private bool isGameOver = false;

    public int CurrentGameLevel{
        get { return _currentGameLevel; }
    }

    private void Awake(){
        Instance = this;
    }

    private void Start()
    {
        rightHandGrabInteractor.WhenInteractableSet.Action += (HandGrabInteractable interactable) => { OnInteractableSelected(interactable, true); };
        rightHandGrabInteractor.WhenInteractableUnset.Action += (HandGrabInteractable interactable) => { OnInteractableUnSelected(interactable, true); };
        leftHandGrabInteractor.WhenInteractableSet.Action += (HandGrabInteractable interactable) => { OnInteractableSelected(interactable, false); };
        leftHandGrabInteractor.WhenInteractableUnset.Action += (HandGrabInteractable interactable) => { OnInteractableUnSelected(interactable, false); };
        if (PhotonNetwork.IsMasterClient)
        {
            mainUi.SetActive(true);
            startGameUiCtrl.SetDataInUI();
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Fire?.Invoke();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            SecondaryFire?.Invoke();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (DebugLogManager.Instance.IsLogWindowVisible)
            {
                DebugLogManager.Instance.HideLogWindow();
            }
            else
            {
                DebugLogManager.Instance.ShowLogWindow();
            }
        }
        _timeSpent += Time.deltaTime;
    }
    private void OnInteractableSelected(HandGrabInteractable interactable, bool isRightHand)
    {
        if(interactable.tag == "Weapon") {
            WeaponCtrl weaponCtrl = interactable.GetComponentInParent<WeaponCtrl>();
            SetEventForFiring(weaponCtrl, isRightHand);
        }
    }

    private void OnInteractableUnSelected(HandGrabInteractable interactable, bool isRightHand)
    {
        if (interactable.tag == "Weapon")
        {
            WeaponCtrl weaponCtrl = interactable.GetComponentInParent<WeaponCtrl>();
            RemoveEventForFiring(weaponCtrl, isRightHand );
        }
    }

    private void SetEventForFiring(WeaponCtrl weaponCtrl, bool isRightHand)
    {
        if (isRightHand)
        {
            Fire.AddListener(weaponCtrl.SpawnBall);
        }
        else
        {
            SecondaryFire.AddListener(weaponCtrl.SpawnBall);
        }
    }

    private void RemoveEventForFiring(WeaponCtrl weaponCtrl, bool isRightHand)
    {
        if (isRightHand)
        {
            Fire.RemoveListener(weaponCtrl.SpawnBall);
        }
        else
        {
            SecondaryFire.RemoveListener(weaponCtrl.SpawnBall);
        }
    }

    public void UpdateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void ChangeMeshParentToRobotView()
    {
        robotViewEffectMesh.SetEffectObjectsParent(robotMeshVieTransform);
        Transform[] robotViewChildObjects = robotMeshVieTransform.GetComponentsInChildren<Transform>();
        int robotViewLayer = LayerMask.NameToLayer("RobotMeshViewLayer");
        foreach(Transform child in robotViewChildObjects)
        {
            child.gameObject.layer = robotViewLayer;
        }
    }

    public void StartGameOnMasterClient()
    {
        StartGame();
    }

    public void StartGame(){
        Invoke("StartNextLevel", 2f);
    }

    public void StartNextLevel(){
        if (photonView.IsMine)
        {
            photonView.RPC("IncreaseCurrentLevel", RpcTarget.All);
        }
    }

    [PunRPC]
    private void IncreaseCurrentLevel()
    {
        _currentGameLevel++;
        if (_currentGameLevel > _maxLevel)
        {
            isGameOver = true;
            gameUiOverCtrl.SetDataInUI(new GameUiOverCtrl.UiData()
            {
                score = _score,
                time = (int)_timeSpent
            });
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                cubeSpawnerCtrl.SpawnCubesRandomly((int)(cubeSpawnerCtrl.CubesToCreate * 1.3));
            }
        }
    }

    public void IncreaseScoreByOne()
    {
        _score++;
    }

    public void OnDestroOfCube(){
        cubeSpawnerCtrl.OnDestroyEachCube(StartGame);
        IncreaseScoreByOne();
    }
}
