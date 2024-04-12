using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public static UnityEvent Fire = new UnityEvent();
    public static UnityEvent SecondaryFire = new UnityEvent();

    public GrabInteractor rightHandGrabInteractable;
    public GrabInteractor leftHandGrabInteractable;
    public DistanceGrabInteractor leftHandDistanceGrabInteractable;
    public DistanceGrabInteractor rightHandDistanceGrabInteractable;

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

}
