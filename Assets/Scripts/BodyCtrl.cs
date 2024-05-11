using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class BodyCtrl : MonoBehaviour
{
    [Serializable]
    public struct Mapping
    {
        public Transform vrTarget;
        public Transform boneTarget;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        public void MapTargets()
        {
            boneTarget.position = vrTarget.TransformPoint(positionOffset);
            boneTarget.rotation = vrTarget.rotation * Quaternion.Euler(rotationOffset);
        }
    }

    public Mapping headset;
    public Mapping rightController;
    public Mapping leftController;

    public Transform headConstraint;
    private Vector3 offset;

    private OVRCameraRig ovrCameraRig;

    void Start()
    {
        ovrCameraRig = GameObject.FindObjectOfType<OVRCameraRig>();
        Debug.Log(ovrCameraRig.gameObject.name + " ovrCameraRig name");
        headset.vrTarget = ovrCameraRig.centerEyeAnchor;
        rightController.vrTarget = ovrCameraRig.rightHandAnchor;
        leftController.vrTarget = ovrCameraRig.leftHandAnchor;
        offset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = headConstraint.position + offset;
        transform.forward = Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized;

        headset.MapTargets();
        rightController.MapTargets();
        leftController.MapTargets();
    }
}
