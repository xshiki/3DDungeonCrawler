using UnityEngine.AI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemyAI : EnemyAI
{

    public Projectile projectile;
    public Spell spellToCast;
 
    public GameObject aim;
  


    // Attack the player by reducing their health and knocking them back
    public override void AttackPlayer()
    {
        transform.LookAt(player);
        enemy.speed = 0;
        if (!alreadyAttacked)
        {   

            if(aim != null)
            {
                Instantiate(projectile, aim.transform.position, transform.rotation);
            }
            else
            {
                Instantiate(projectile, transform.position, transform.rotation);
            }
           
            alreadyAttacked = true;
            animator.Play("Ranged Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}