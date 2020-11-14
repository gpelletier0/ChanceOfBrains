using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] public List<Transform> spawnPointList = new List<Transform>();

    private void Awake()
    {
        foreach(Transform t in GetComponent<Transform>())
        {
            spawnPointList.Add(t);
        }
    }
}
