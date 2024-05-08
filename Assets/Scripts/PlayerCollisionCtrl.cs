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
        Debug.Log(collision.gameObject.tag + " collision tag");
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log(collision.gameObject.tag + " bullet tag");
            PlayerCtrl.Instance.ReduceHealthAndShieldOfPlayer(damageToBeTakenFromBullet);
            Destroy(collision.gameObject);
        }
    }
}
