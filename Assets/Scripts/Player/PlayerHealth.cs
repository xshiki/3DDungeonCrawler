using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the player
    public int currentHealth; // Current health of the player

    void Start()
    {
        currentHealth = maxHealth; // Set the current health to the maximum health on start
    }

    // Function to reduce the player's health by a specified amount
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // If the player's health is less than or equal to 0, trigger death
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GetComponent<PlayerRespawn>().Respawn();
        }
    }


}