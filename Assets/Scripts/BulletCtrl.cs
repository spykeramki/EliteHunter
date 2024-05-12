using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    private Rigidbody rb;

    public PhotonView photonView;

    public Rigidbody RigidBody
    {
        get { return rb; }
    }

    public GameObject particleEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Weapon")
        {
            photonView.RPC("InstantiateParticleEffects", RpcTarget.All);
            if (collision.gameObject.tag != "PlayerBody")
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    void InstantiateParticleEffects()
    {
        Instantiate(particleEffect, transform.position, Quaternion.identity);
    }
}
