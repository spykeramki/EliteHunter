using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionCtrl : MonoBehaviour
{
    public PlayerCtrl playerCtrl;

    [SerializeField]
    private float damageToBeTakenFromBullet;

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient && collision.gameObject.tag == "Bullet")
        {
            PlayerCtrl.Instance.ReduceHealthAndShieldOfPlayer(damageToBeTakenFromBullet);
            Destroy(collision.gameObject);
        }
    }
}
