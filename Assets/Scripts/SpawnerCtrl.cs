using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCtrl : MonoBehaviour
{
    public FindSpawnPositions cubeSpawner;

    private int _cubesToCreate = 5;
    public int CubesToCreate{
        get { return _cubesToCreate;}
    }


    public void SpawnCubesRandomly(int m_cubesToCreate){
        _cubesToCreate = m_cubesToCreate;
        cubeSpawner.SpawnAmount = _cubesToCreate;
        cubeSpawner.StartSpawn();
    }

    public void OnDestroyEachCube( Action spawnNextLevelCubes){
        Debug.Log(transform.childCount + " transform.childCount");
        if(transform.childCount <= 1){
            spawnNextLevelCubes?.Invoke();
        }
    }

    public void DestroyEachCube( ){
        Destroy(transform.GetChild(0).gameObject);
    }
}
