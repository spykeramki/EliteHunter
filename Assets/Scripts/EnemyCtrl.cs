using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyCtrl : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator animator;

    private bool isWalking = false;
    private bool isPetOnCall = false;

    private float health = 100f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OnClickCallPetBot();
    }

    private void Update()
    {
        /*if (agent.isOnNavMesh && (Vector3.Distance(PlayerCtrl.Instance.transformComp.position, transform.position) > 1))
        {
            if (!isOpened)
            {
                isOpened = true;
                animator.SetBool("Open_Anim", true);
            }
            if (!isWalking)
            {
                isWalking = !isWalking;
                animator.SetBool("Open_Anim", true);
                animator.SetBool("Walk_Anim", true);
            }

        }
        else
        {
            if (isWalking)
            {
                isWalking = !isWalking;
                animator.SetBool("Walk_Anim", false);
                animator.SetBool("Open_Anim", false);
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            OnClickCallPetBot();
        }*/

        
    }

    private void LateUpdate()
    {
        if (isWalking)
        {
            agent.destination = PlayerCtrl.Instance.transformComp.position;
            if (Vector3.Distance(PlayerCtrl.Instance.transformComp.position, transform.position) <= 0.5f)
            {
                isWalking = false;
                isPetOnCall = false;
                animator.SetBool("Walk_Anim", false);
                animator.SetBool("Open_Anim", false);
            }
        }
    }

    public void OnClickCallPetBot()
    {
        if(!isPetOnCall && (Vector3.Distance(PlayerCtrl.Instance.transformComp.position, transform.position) > 0.5f))
        {
            isPetOnCall = true;
            animator.SetBool("Open_Anim", true);
        }
    }

    public void OnOpenAndWalkRobot()
    {
        isWalking = true;
        animator.SetBool("Walk_Anim", true);
    }

    private void ReduceHealth(float m_health){
        health -= m_health;
        if(health <= 0){
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision){
        
        if(collision.gameObject.tag == "Bullet"){
            ReduceHealth(35f);
        }
    }
}
