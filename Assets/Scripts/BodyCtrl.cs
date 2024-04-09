using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            boneTarget.position = vrTarget.position + positionOffset;
            boneTarget.rotation = vrTarget.rotation * Quaternion.Euler(rotationOffset);
        }
    }

    public Mapping headset;
    public Mapping rightController;
    public Mapping leftController;

    public Transform headConstraint;
    private Vector3 offset;

    void Start()
    {
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
