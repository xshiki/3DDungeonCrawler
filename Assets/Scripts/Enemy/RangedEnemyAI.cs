using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))] 
public class RangedEnemyAI : MonoBehaviour
{
    Transform player; // Reference to the player's transform
    NavMeshAgent enemy;
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
        enemy = GetComponent<NavMeshAgent>();
     
    }

    void FixedUpdate()
    {   


     
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLM);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLM);

        //if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();





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
        enemy.SetDestination(player.position);


      
        /*
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
        */

    }



    // Attack the player by reducing their health and knocking them back
    void AttackPlayer()
    {

        transform.LookAt(player);
        enemy.SetDestination(transform.position);

        if (!alreadyAttacked)
        {

            Instantiate(projectile, transform.position, transform.rotation);   
            alreadyAttacked = true;
           
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }




    }
}




