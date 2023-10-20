using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemyAI : EnemyAI
{

    
    // Attack the player by reducing their health and knocking them back
    public override void AttackPlayer()
    {
        enemy.speed = 0;
        transform.LookAt(player);
        //find the vector pointing from our position to the target
        Vector3 direction = (transform.position - player.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 0.5f);


        if (!alreadyAttacked)
        {

            
            alreadyAttacked = true;
    
            Vector3 pushForceVector = direction * pushForce;
            Rigidbody playerRigidbody = player.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.AddForce(pushForceVector, ForceMode.Impulse);
            animator.Play("Attack");
            player.GetComponent<PlayerRessource>().TakeDamage(attackDamage);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
       
    }
}
