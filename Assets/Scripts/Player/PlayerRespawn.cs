
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint; // The location where the player will respawn
    public PlayerRessource playerHealth; // Reference to the player's health script
    public GameObject Player;



    private void Awake()
    {
        Player = GameObject.Find("Player");
        respawnPoint = transform;
        playerHealth = Player.GetComponent<PlayerRessource>();
        Player.transform.position = respawnPoint.position;

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
