using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFootIK : MonoBehaviour
{
    private Animator _anim;


    public Vector3 footOffset;
    private float rightFootPositionWeight = 1;


    private float rightFootRotationWeight = 1;

    private float leftFootPositionWeight = 1;
    private float leftFootRotationWeight = 1;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        /*Vector3 rightFootPos = _anim.GetIKPosition(AvatarIKGoal.RightFoot);
        RaycastHit hit;

        if(Physics.Raycast(rightFootPos + Vector3.up, Vector3.down, out hit))
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPositionWeight);
            _anim.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + footOffset);

            Quaternion rightFootRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            _anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotationWeight);
            _anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
        }
        else
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
        }
        Debug.DrawRay(rightFootPos + Vector3.up, Vector3.down);
        Vector3 leftFootPos = _anim.GetIKPosition(AvatarIKGoal.LeftFoot);

        if (Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out hit))
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPositionWeight);
            _anim.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + footOffset);

            Quaternion leftFootRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotationWeight);
            _anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
        }
        else
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
        }*/
    }
}
