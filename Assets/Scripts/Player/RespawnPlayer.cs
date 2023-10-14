using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{

    public Transform spawnPoint;
    void OnTriggerEnter(Collider other)
    {
        spawnPoint = GameObject.Find("Player Start Room").transform;
        other.transform.position = spawnPoint.position + new Vector3(0, 1f, 0); ;
    }
}
