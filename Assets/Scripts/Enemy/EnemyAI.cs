using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public NavMeshAgent enemy;
    protected Animator animator;
    public float walkSpeed = 3f; // Speed at which the enemy moves towards the player
    public float runSpeed = 5f;
    public float pushForce = 8f;
    public int attackDamage = 5;

    EnemyManager enemyManager;

    public float wayPointRange = 10f;

    public float viewRadius = 15f;
    [Range(0, 360)]
    public float viewAngle = 90;
    public float timeBetweenAttacks;
    public bool alreadyAttacked = false;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public Vector3 currentPlayerPosition;
    public Vector3 lastSeenPlayerPositon = Vector3.zero;
    public LayerMask playerLM;
    public LayerMask obstaclesLM;


    public List<Vector3> wayPoints = new List<Vector3>();
    public int numberOfWayPoints = 8;
    public int currentWaypointIndex;

    public float startWaitTime = 2;
    public float timeToRotate = 2;
    float waitTime;

    float rotateTime;

    bool playerNear;


    [SerializeField]
    private bool isPatrol;
    bool caughtPlayer;


    private void Start()
    {
        attackDamage = GetComponent<EnemyManager>().attackDamage;
    }

    private void Awake()
    {

        enemyManager = GetComponent<EnemyManager>();
        attackDamage = GetComponent<EnemyManager>().attackDamage;
        gameObject.tag = "Enemy";
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        StartCoroutine(CheckForPlayer());
        currentWaypointIndex = 0;
        rotateTime = timeToRotate;
        playerNear = false;
        caughtPlayer = false;
        isPatrol = true;
        enemy.isStopped = false;
        enemy.speed = walkSpeed;
        playerLM |= 0x1 << 9;
        obstaclesLM |= 0x1 << 11;
        animator = GetComponent<Animator>();

      

    }
    public void SetBossSpawnPoint(Vector3 position)
    {
        wayPoints.Add(position);
    }
    public void SetSpawnPosition(Vector3 positon)
    {
    
        wayPoints.Add(positon);
        CreateNewWayPoint();
    }

    void FixedUpdate()
    {
        //CheckForPlayer
        if (enemyManager.IsDead) {  return; }
        if (enemyManager.GotHit) { transform.LookAt(player); enemy.SetDestination(player.position); }
        if (player == null) { return; }
        float dstToPlayer = Vector3.Distance(transform.position, player.position);
        playerInAttackRange = dstToPlayer < attackRange ? true : false;
        if (enemy.speed == 0)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 1);
        }
        //if (!playerInSightRange && !playerInAttackRange) Patroling();



        if (!isPatrol)
        {
            ChasePlayer();
            if (playerInAttackRange) { AttackPlayer(); }
        }
        else
        {
            enemyManager.ResetGotHit();
            Patroling();
        }
        
    }


    public void CreateNewWayPoint()
    {

        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
       
        NavMeshHit Hit;
        while(wayPoints.Count < numberOfWayPoints)
        {
            int VertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);
            if (NavMesh.SamplePosition(triangulation.vertices[VertexIndex], out Hit, 20f, 11))
            {
                wayPoints.Add(Hit.position);

            }
        }  
    }

    private IEnumerator CheckForPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);

        while (true)
        {
            yield return wait;
            FOVCheck();

        }
    }


    private void FOVCheck()
    {

        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerLM);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstaclesLM))
                {
                    playerInSightRange = true;
                    isPatrol = false;
                    caughtPlayer = true;

                }
                else
                {
                    playerInSightRange = false;
                }
            }

            if(Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                playerInSightRange = false;
                currentPlayerPosition = player.position/2;

       
            }

            if (playerInSightRange)
            {

                currentPlayerPosition = player.position; 
            }


        }
    }




    private void Patroling()
    {
        if (playerNear)
        {
           
            if (rotateTime <= 0)
            {
                Move(walkSpeed);
                LookingForPlayer();
            }
            else
            {
                
                Stop();
                rotateTime -= Time.deltaTime;
            }
        }
        else
        {
           
            playerNear = false;           //  The player is no near when the enemy is platroling
            lastSeenPlayerPositon = Vector3.zero;
            enemy.SetDestination(wayPoints[currentWaypointIndex]);    //  Set the enemy destination to the next waypoint
            if (enemy.remainingDistance <= enemy.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (waitTime <= 0)
                {
                    NextPoint();
                    Move(walkSpeed);
                    waitTime = startWaitTime;
                    
                }
                else
                {
                    Stop();
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }


    void LookingForPlayer()
    {
            if (waitTime <= 0)
            {
                playerNear = false;
                Move(walkSpeed);
                
                float randomZ = Random.Range(-wayPointRange, wayPointRange);
                float randomX = Random.Range(-wayPointRange, wayPointRange);

            Vector3 walkPoint = currentPlayerPosition + new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
                enemy.SetDestination(walkPoint);
                
                rotateTime = timeToRotate;
                
            }
            else
            {
                Stop();
                waitTime -= Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 360, 0), Time.deltaTime);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ChasePlayer()
    {

        /*
     
        enemy.speed = runSpeed;
        enemy.SetDestination(player.position);

       
        
        */

        lastSeenPlayerPositon = Vector3.zero;
        //transform.LookAt(player);

        //ChasePlayer
        if (caughtPlayer)
        {
            Move(runSpeed);
            enemy.SetDestination(currentPlayerPosition);
           
        }
        //If enemy has arrived at the player position, wait, after waiting return to patroling
        if(enemy.remainingDistance <= enemy.stoppingDistance)
        {
           
            if(waitTime <= 0  && caughtPlayer)
            {

                isPatrol = true;
                playerNear = true;
                Move(walkSpeed);
                rotateTime = timeToRotate;
                waitTime = startWaitTime;
                caughtPlayer = false;
                enemy.SetDestination(wayPoints[currentWaypointIndex]);

              
            }else 
            {

                    Stop();
                    waitTime -= Time.deltaTime;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 360, 0), 5f * Time.deltaTime);
                  
            }
        }



    }
    void Stop()
    {
        enemy.isStopped = true;
        enemy.speed = 0;
    }
    void Move(float speed)
    {
        enemy.isStopped = false;
        enemy.speed = speed;
    }

    public void NextPoint()
    {
       
        currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Count;
         
        enemy.SetDestination(wayPoints[currentWaypointIndex]);
    }

    public virtual void AttackPlayer() { Debug.Log("ImplementAttacking"); }
}