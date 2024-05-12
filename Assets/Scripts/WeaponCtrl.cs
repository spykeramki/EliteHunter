using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponCtrl : MonoBehaviour
{
    public GameObject paintBall;

    public Transform bulletSpawnPoint;

    public float bulletSpeed;

    public void SpawnBall()
    {
        GameObject ball = PhotonNetwork.Instantiate(paintBall.name, bulletSpawnPoint.position, Quaternion.identity);
        BulletCtrl ballCtrl = ball.GetComponent<BulletCtrl>();

        ballCtrl.RigidBody.velocity = bulletSpawnPoint.forward * bulletSpeed;
    }
}
