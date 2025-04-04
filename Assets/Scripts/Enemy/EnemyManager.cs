using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Pool;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(LootTable))]
[RequireComponent (typeof(Animator))]
public class EnemyManager : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the enemy
    public int currentHealth = 10; // Current health of the enemy
    public int attackDamage = 5;
    public int experiencePoints = 10;
    public FloorTextOverlay floor;
    public GameObject floatingText;
    Animator animator;
    NavMeshAgent enemy;
    public UnityAction OnDie;
    public bool GotHit => gotHit;
    bool gotHit = false;
    [SerializeField] public ExperienceManager experienceManager;

    private ObjectPool <GameObject> _enemyPool;
    
    public bool IsDead => isDead;
    bool isDead = false;
    private void Awake()
    {
      experienceManager = GameObject.Find("Player").GetComponent<ExperienceManager>();
      floor = GameObject.Find("Floor Counter").GetComponent<FloorTextOverlay>();
      animator = GetComponent<Animator>();
      currentHealth = maxHealth;
      gotHit = false;
      enemy = GetComponent<NavMeshAgent>();
    }



    //Use to scale up enemy stats over a animation curve
    public void EnemyScaler(ScalingScriptableObject Scaling, int level)
    {

        maxHealth = Mathf.FloorToInt(maxHealth * Scaling.healthCurve.Evaluate(level));
        attackDamage = Mathf.FloorToInt(attackDamage * Scaling.damageCurve.Evaluate(level));
        experiencePoints = Mathf.FloorToInt(experiencePoints * Scaling.experienceCurve.Evaluate(level));
        currentHealth = maxHealth;
    }

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _enemyPool = pool;
    }

    public void ResetGotHit()
    {
        gotHit = false;
    }
    // Function to reduce the player's health by a specified amount
    public bool TakeDamage(int damage)
    {   
        currentHealth -= damage;
        if(floatingText != null)
        {
            ShowFloatingText(damage);
        }
      
        gotHit = true;
        if (currentHealth <= 0)
        {
            Die();
        }

        return true;
    }


    void ShowFloatingText(int damage)
    {
    
        var floatGO = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        floatGO.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    // Function to trigger death
    public void Die()
    {

        isDead = true;
        enemy.isStopped = true;
        enemy.ResetPath();
        enemy.updateRotation = false;
        enemy.velocity = Vector3.zero;
        enemy.speed = 0;
        GetComponent<LootTable>().InstantiateLoot(transform.position);
        experienceManager.AddExperience(experiencePoints);
        animator.SetTrigger("OnDeath");
        animator.CrossFade("Death", 0f);
        FindAnyObjectByType<AudioManager>().PlayRandomized("Mob Death", 0.4f, 0.6f, 0.6f, 1.2f);

        //_enemyPool.Release(gameObject);
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        
        yield return new WaitForSeconds(2f);
        OnDeathAnimationFinished();
    }
    
    public void OnDeathAnimationFinished()
    {
        if (_enemyPool != null)
        {
        _enemyPool.Release(gameObject);

        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}