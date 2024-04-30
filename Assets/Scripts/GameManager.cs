using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static UnityEvent Fire = new UnityEvent();
    public static UnityEvent SecondaryFire = new UnityEvent();

    public HandGrabInteractor rightHandGrabInteractor;
    public HandGrabInteractor leftHandGrabInteractor;

    public NavMeshSurface navMeshSurface;

    private WeaponCtrl _rightHandWeapon;
    private WeaponCtrl _leftHandWeapon;

    public EffectMesh robotViewEffectMesh;
    public Transform robotMeshVieTransform;

    public SpawnerCtrl cubeSpawnerCtrl;

    public PlayerRoboCamCtrl playerRoboCamCtrl;

    private bool isRobotView = false;

    private int _currentGameLevel = 0;

    private int testInt = 0;

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
        if(OVRInput.GetDown(OVRInput.Button.One)){
            isRobotView = !isRobotView;
            playerRoboCamCtrl.SetRobotControlStatus(isRobotView);
            playerRoboCamCtrl.SetRobotToIdle();
            PlayerCtrl.Instance.SetRoboCamCanvas(isRobotView);
        }
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
            _rightHandWeapon = weaponCtrl;
            Fire.AddListener(weaponCtrl.SpawnBall);
        }
        else
        {
            _leftHandWeapon = weaponCtrl;
            SecondaryFire.AddListener(weaponCtrl.SpawnBall);
        }
    }

    private void RemoveEventForFiring(WeaponCtrl weaponCtrl, bool isRightHand)
    {
        if (isRightHand)
        {
            _rightHandWeapon = null;
            Fire.RemoveListener(weaponCtrl.SpawnBall);
        }
        else
        {
            _leftHandWeapon = null;
            SecondaryFire.RemoveListener(weaponCtrl.SpawnBall);
        }
    }

    public void UpdateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    private void GetRoomsData()
    {
        List<MRUKRoom> roomsList = MRUK.Instance.GetRooms();

        foreach (MRUKRoom room in roomsList)
        {
            Debug.Log(room.name + " room name");

        }
        MRUKRoom firstRoom = roomsList[0];
        Debug.Log(firstRoom.name + " firstRoom name");
        List<MRUKAnchor> roomAnchors = firstRoom.GetRoomAnchors();

        foreach (MRUKAnchor roomAnchor in roomAnchors)
        {
            /*Transform righthandTransform = _rightHandWeapon.transform;
            Ray ray = new Ray(righthandTransform.position, righthandTransform.forward);
            RaycastHit hit;
            roomAnchor.Raycast(ray,10f, out hit);*/
            Vector3 anchorCenter = roomAnchor.GetAnchorCenter();
            Debug.Log(anchorCenter + " anchorCenter, " + roomAnchor.name + " room anchor name");
            List<string> roomAnchorLabels = roomAnchor.AnchorLabels;
            foreach(string roomAnchorLabel in roomAnchorLabels)
            {
                Debug.Log(roomAnchorLabel + " roomAnchorLabel " + roomAnchor.name + " room anchor name");
            }
        }
        MRUKAnchor firstRoomFloorAnchor =  firstRoom.GetFloorAnchor();
        Vector3 randomPosOnWall;
        Vector3 randomPosNormalOnWall;
        bool hasRandomPosOnWall = firstRoom.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, 0.5f, 
            LabelFilter.Included(new List<string>() { "WALL_FACE"}), out randomPosOnWall, out randomPosNormalOnWall);
        Debug.Log(hasRandomPosOnWall + " hasRandomPosOnWall, " + randomPosOnWall + " randomPosOnWall " + randomPosNormalOnWall + " randomPosNormalOnWall " + firstRoom.name + " firstRoom name");
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

    public void StartGame(){
        Invoke("StartNextLevel", 2f);
    }

    public void StartNextLevel(){
        _currentGameLevel ++;
        cubeSpawnerCtrl.SpawnCubesRandomly((int)(cubeSpawnerCtrl.CubesToCreate * 1.3));
    }

    public void OnDestroOfCube(){
        cubeSpawnerCtrl.OnDestroyEachCube(StartGame);
    }
}
