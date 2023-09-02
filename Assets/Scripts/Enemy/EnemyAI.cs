using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
  
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    public float pushForce = 5f;
    public int attackDamage = 10; // Amount of damage the enemy deals to the player when attacking

    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public Projectile projectile;
    public Spell spellToCast;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public LayerMask playerLM;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
     
    }

    void FixedUpdate()
    {   
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, playerLM);
        foreach (Collider col in colliders)
        {
            if (col.gameObject == player.gameObject)
            {   
                // Player is within detection radius, chase them
                ChasePlayer();
                return;
            }
        }


       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, sightRange);
        Gizmos.DrawSphere(transform.position, attackRange);

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void ChasePlayer()
    {
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, playerLM);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == player.gameObject)
            {
                // Player is within detection radius, chase them
                transform.position += Vector3.zero;
                AttackPlayer();
                return;
            }
        }

    }



    // Attack the player by reducing their health and knocking them back
    void AttackPlayer()
    {
       
      
        if (!alreadyAttacked)
        {

            Instantiate(projectile, transform.position, transform.rotation);   
            alreadyAttacked = true;
           
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }




        /*
        // If the attack timer has reached the attack interval
          attackTimer += Time.deltaTime; 
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
        */
    }
}




