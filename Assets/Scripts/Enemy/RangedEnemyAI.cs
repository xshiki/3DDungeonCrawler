using UnityEngine.AI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemyAI : EnemyAI
{

    public Projectile projectile;
    public Spell spellToCast;
  


    // Attack the player by reducing their health and knocking them back
    public override void AttackPlayer()
    {
        transform.LookAt(player);
        enemy.speed = 0;
        if (!alreadyAttacked)
        {
            Instantiate(projectile, transform.position, transform.rotation);
            alreadyAttacked = true;

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}