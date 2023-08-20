
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint; // The location where the player will respawn
    public PlayerHealth playerHealth; // Reference to the player's health script

    void Update()
    {
        // If the player's health is 0 or below and they are not already respawning
        if (playerHealth.currentHealth <= 0 && !IsInvoking("Respawn"))
        {
            // Start the respawn process
            Invoke("Respawn", 3f);
        }
    }

    // Function to respawn the player
    public void Respawn()
    {
        // Set the player's position to the respawn point
        transform.position = respawnPoint.position;

        // Set the player's health to the maximum value
        playerHealth.currentHealth = playerHealth.maxHealth;
    }
}
