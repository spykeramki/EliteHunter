using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPaintBall : MonoBehaviour
{
    public PaintBallCtrl paintBall;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            PaintBallCtrl ball = Instantiate(paintBall, transform.position, Quaternion.identity);
            ball.RigidBody.velocity = transform.forward * 5f;
        }

    }
}
