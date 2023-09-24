using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(LootTable))]
[RequireComponent (typeof(Animator))]
public class EnemyManager : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the enemy
    public int currentHealth = 10; // Current health of the enemy
    public int attackDamage = 10;
    public int experiencePoints = 10;
    public FloorTextOverlay floor;
    Animator animator;


    [SerializeField] public ExperienceManager experienceManager;

    private ObjectPool<GameObject> _enemyPool;

    private void Awake()
    {
      experienceManager = GameObject.Find("Player").GetComponent<ExperienceManager>();
      floor = GameObject.Find("Floor Counter").GetComponent<FloorTextOverlay>();
       animator = GetComponent<Animator>();
      currentHealth = maxHealth;
     
    }



    //Use to scale up enemy stats over a animation curve
    public void EnemyScaler(ScalingScriptableObject Scaling, int level)
    {

        maxHealth = Mathf.FloorToInt(maxHealth * Scaling.healthCurve.Evaluate(level));
        attackDamage = Mathf.FloorToInt(attackDamage * Scaling.damageCurve.Evaluate(level));
        experiencePoints = Mathf.FloorToInt(attackDamage * Scaling.experienceCurve.Evaluate(level));
        currentHealth = maxHealth;
    }

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _enemyPool = pool;
    }


    // Function to reduce the player's health by a specified amount
    public bool TakeDamage(int damage)
    {   
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }

        return true;
    }

    // Function to trigger death
    public void Die()
    {
      
        GetComponent<LootTable>().InstantiateLoot(transform.position);
        experienceManager.AddExperience(experiencePoints);
        animator.SetTrigger("OnDeath");
        //_enemyPool.Release(gameObject);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        
        yield return new WaitForSeconds(1.5f);
        OnDeathAnimationFinished();
    }
    
    public void OnDeathAnimationFinished()
    {
        _enemyPool.Release(gameObject);
    }
}