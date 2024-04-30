using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCubeCtrl : MonoBehaviour
{
    public GameObject thisGo;

    private int id = -1;
    public int Id{
        get{ return id; }
    }

    public void SetCubeId(int m_id){
        id = m_id;
    }

    private void OnDestroy(){
        GameManager.Instance.OnDestroOfCube();
    }
}