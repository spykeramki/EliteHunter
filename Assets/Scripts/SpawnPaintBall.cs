using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPaintBall : MonoBehaviour
{
    public PaintBallCtrl paintBall;

    private void Update()
    {

    }

    public void SpawnBall()
    {
        PaintBallCtrl ball = Instantiate(paintBall, transform.position, Quaternion.identity);
        ball.RigidBody.velocity = transform.forward * 5f;
    }
}
