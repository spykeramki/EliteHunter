using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    public BulletCtrl paintBall;

    public Transform bulletSpawnPoint;

    public void SpawnBall()
    {
        BulletCtrl ball = Instantiate(paintBall, bulletSpawnPoint.position, Quaternion.identity);

        ball.RigidBody.velocity = bulletSpawnPoint.forward * 5f;
    }
}
