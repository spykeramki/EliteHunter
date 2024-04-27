using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityEvent Fire = new UnityEvent();
    public static UnityEvent SecondaryFire = new UnityEvent();

    public GrabInteractor rightHandGrabInteractable;
    public GrabInteractor leftHandGrabInteractable;
    public DistanceGrabInteractor leftHandDistanceGrabInteractable;
    public DistanceGrabInteractor rightHandDistanceGrabInteractable;

    public NavMeshSurface navMeshSurface;

    private WeaponCtrl _rightHandWeapon;
    private WeaponCtrl _leftHandWeapon;

    private void Start()
    {
        rightHandGrabInteractable.WhenInteractableSet.Action += (GrabInteractable interactable) => { OnInteractableSelected(interactable, true); };
        rightHandGrabInteractable.WhenInteractableUnset.Action += (GrabInteractable interactable) => { OnInteractableUnSelected(interactable, true); };
        leftHandGrabInteractable.WhenInteractableSet.Action += (GrabInteractable interactable) => { OnInteractableSelected(interactable, false); };
        leftHandGrabInteractable.WhenInteractableUnset.Action += (GrabInteractable interactable) => { OnInteractableUnSelected(interactable, false); };

        rightHandDistanceGrabInteractable.WhenInteractableSet.Action += (DistanceGrabInteractable interactable) => { OnDistanceInteractableSelected(interactable, true); };
        rightHandDistanceGrabInteractable.WhenInteractableUnset.Action += (DistanceGrabInteractable interactable) => { OnDistanceInteractableUnSelected(interactable, true); };
        leftHandDistanceGrabInteractable.WhenInteractableSet.Action += (DistanceGrabInteractable interactable) => { OnDistanceInteractableSelected(interactable, false); };
        leftHandDistanceGrabInteractable.WhenInteractableUnset.Action += (DistanceGrabInteractable interactable) => { OnDistanceInteractableUnSelected(interactable, false); };
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
        if (OVRInput.GetDown(OVRInput.Button.Three)){
            GetRoomsData();
        }
    }


    private void OnInteractableSelected(GrabInteractable interactable, bool isRightHand)
    {
        if(interactable.tag == "Weapon") {
            WeaponCtrl weaponCtrl = interactable.GetComponent<WeaponCtrl>();
            SetEventForFiring(weaponCtrl, isRightHand);
        }
    }

    private void OnInteractableUnSelected(GrabInteractable interactable, bool isRightHand)
    {
        if (interactable.tag == "Weapon")
        {
            WeaponCtrl weaponCtrl = interactable.GetComponent<WeaponCtrl>();
            RemoveEventForFiring(weaponCtrl, isRightHand );
        }
    }

    private void OnDistanceInteractableSelected(DistanceGrabInteractable interactable, bool isRightHand)
    {
        if (interactable.tag == "Weapon")
        {
            WeaponCtrl weaponCtrl = interactable.GetComponent<WeaponCtrl>();
            SetEventForFiring(weaponCtrl, isRightHand);
        }
    }

    private void OnDistanceInteractableUnSelected(DistanceGrabInteractable interactable, bool isRightHand)
    {
        if (interactable.tag == "Weapon")
        {
            WeaponCtrl weaponCtrl = interactable.GetComponent<WeaponCtrl>();
            RemoveEventForFiring(weaponCtrl, isRightHand);
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
}
