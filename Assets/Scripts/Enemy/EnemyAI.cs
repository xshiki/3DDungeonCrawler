using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float followRange = 10f; // Distance at which the enemy starts following the player
    public float attackRange = 1f; // Distance at which the enemy attacks the player
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    public float pushForce = 5f;
    public int attackDamage = 10; // Amount of damage the enemy deals to the player when attacking
    float attackTimer = 0f; // Timer to track elapsed time between attacks
    float attackInterval = 1f; // Time interval between attacks

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the distance to the player is less than the follow range, start following the player
        if (distanceToPlayer < followRange)
        {
            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        }

        // If the distance to the player is less than the attack range, attack the player
        else if (distanceToPlayer < attackRange)
        {
            AttackPlayer();
        }
    }

    // Attack the player by reducing their health and knocking them back
    void AttackPlayer()
    {
        // Increment the attack timer
        attackTimer += Time.deltaTime;

        // If the attack timer has reached the attack interval
        if (attackTimer >= attackInterval)
        {
            // Reset the attack timer
            attackTimer = 0f;

            // Reduce the player's health
            player.GetComponent<PlayerRessource>().TakeDamage(attackDamage);

            // Get the player's Rigidbody component
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            Vector3 pushDirection = (transform.position - player.position).normalized;
            Vector3 pushForceVector = pushDirection * pushForce;
            // Apply a force to the player in the opposite direction of the enemy
            playerRigidbody.AddForce(pushForceVector, ForceMode.Impulse);
        }
    }
}




