using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

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
                Vector3 direction = player.transform.position - aim.transform.position;
                projectile.GetComponent<Rigidbody>().velocity = direction;
                Quaternion rotation = Quaternion.LookRotation(direction);
                var projectileGO = Instantiate(projectile, aim.transform.position, rotation);
                projectileGO.SetDamage(attackDamage);
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