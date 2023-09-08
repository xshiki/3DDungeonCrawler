using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemyAI : MonoBehaviour
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
    }



    // Attack the player by reducing their health and knocking them back
    void AttackPlayer()
    {
        //find the vector pointing from our position to the target
        Vector3 direction = (player.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 0.5f);

        enemy.SetDestination(transform.position);


        if (!alreadyAttacked)
        {

        
            alreadyAttacked = true;
    
            Vector3 pushForceVector = direction * pushForce;
            Rigidbody playerRigidbody = player.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.AddForce(pushForceVector, ForceMode.Impulse);

            player.GetComponent<PlayerRessource>().TakeDamage(attackDamage);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
       
    }
}
