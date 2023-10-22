using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInsideColliderChecker : MonoBehaviour
{
    public bool isPlayerInside = false;

    public UnityAction onPlayerOutside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            // Handle the player entering the collider here
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            onPlayerOutside?.Invoke();
        }
    }
}
