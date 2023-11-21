using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RespawnPlayer : MonoBehaviour
{

    public Transform spawnPoint;
    void OnTriggerEnter(Collider other)
    {
        spawnPoint = GameObject.Find("Player Start Room").transform;
        if (other.CompareTag("Player"))
        {
            other.transform.position = spawnPoint.position + new Vector3(0, 1f, 0);
        }
        
        other.transform.position =  spawnPoint.position + new Vector3(0, 1f, 0);
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;


    }
}
