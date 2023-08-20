using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRessource : MonoBehaviour
{
    public int maxRessource = 100; // Maximum ressource of the player
    public int currentRessource; // Current ressource of the player

    void Start()
    {
        currentRessource = maxRessource; // Set the current ressource to the maximum ressource on start
    }

    // Function to reduce the player's ressource by a specified amount
    public void TakeDamage(int damage)
    {
        currentRessource -= damage;

       
    }


}