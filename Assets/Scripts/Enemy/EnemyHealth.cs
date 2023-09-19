using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(LootTable))]
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the enemy
    public int currentHealth = 10; // Current health of the enemy
    public int enemyLevel = 1;



    [SerializeField] public ExperienceManager experienceManager;

    private ObjectPool<GameObject> _enemyPool;

    private void Awake()
    {
      experienceManager = GameObject.Find("Player").GetComponent<ExperienceManager>();
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
        experienceManager.AddExperience(10);

        _enemyPool.Release(gameObject);
    }
}