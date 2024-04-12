using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    private Rigidbody rb;

    public Rigidbody RigidBody
    {
        get { return rb; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
