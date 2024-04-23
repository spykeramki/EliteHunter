using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoboCamCtrl : MonoBehaviour
{

    private Animator _anim;
    public Vector2 speedVec;

    public float rotateSpeed;

    private bool _controlRobot = false;
    // Start is called before the first frame update
    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_controlRobot){
            Vector2 movementVector = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            Vector2 rotateVector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            MoveRobot(movementVector);
            RotateRobot(rotateVector);
        }
    }

    private void MoveRobot(Vector2 m_movementVec){
        transform.Translate(m_movementVec.x * speedVec.x * Time.deltaTime, 0f, m_movementVec.y * speedVec.y * Time.deltaTime);
        if(m_movementVec.x >0.1 || m_movementVec.y >0.1){
            _anim.SetFloat("AnimSpeed",1f);
            _anim.SetBool("Open_Anim", true);   
            _anim.SetBool("Walk_Anim", true);
        }
        else if(m_movementVec.y <= -0.1){
            _anim.SetFloat("AnimSpeed",-1f);
            _anim.SetBool("Open_Anim", true);   
            _anim.SetBool("Walk_Anim", true);
        }
        else{
            _anim.SetBool("Walk_Anim",false);
        }
    }

    private void RotateRobot(Vector2 m_rotateVec){
        transform.Rotate(new Vector3(0f, m_rotateVec.x * rotateSpeed * Time.deltaTime, 0f), Space.Self);
    }

    public void SetRobotControlStatus(bool m_isRobot){
        _controlRobot = m_isRobot;
    }

    public void SetRobotToIdle(){
        if(!_controlRobot){
            _anim.SetBool("Open_Anim", false);   
            _anim.SetBool("Walk_Anim", false);
        }
        else{
            _anim.SetBool("Open_Anim", true);  
        }
    }
}
