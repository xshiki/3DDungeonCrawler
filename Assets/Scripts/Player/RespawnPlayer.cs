using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{

    public Transform spawnPoint;
    void OnTriggerEnter(Collider other)
    {
        other.transform.position = spawnPoint.position;
        Console.WriteLine("Player respawned");
    }
}
