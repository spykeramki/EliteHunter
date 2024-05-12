using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class EnemyCubeCtrl : MonoBehaviour
{
    public GameObject thisGo;

    private PhotonView photonView;

    private int id = -1;
    public int Id{
        get{ return id; }
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SetCubeId(int m_id){
        id = m_id;
    }

    private void OnDestroy(){
        GameManager.Instance.OnDestroOfCube();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            photonView.RequestOwnership();
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
