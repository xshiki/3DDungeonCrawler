using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;



public class EnemySpawner : MonoBehaviour
{

    public int numberOfEnemiesToSpawn = 10;
    public float spawnDelay = 1f;
    ProceduralDungeonGenerator dungeonGenerator;
    public List<GameObject> enemyPrefabs = new();
   

    public ObjectPool<GameObject> _enemyPool;
    public enum SpawnMethod {RoundRobin, Random};
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    public LoadDatabase database;
    public ScalingScriptableObject scaler;
    public int floorCounter;
    public LayerMask layerMask = 11;

    public bool ContinousSpawning = false;

    private int EnemiesAlive = 0;
    private int SpawnedEnemies = 0;

    private void Awake()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();
        _enemyPool = new ObjectPool<GameObject>(CreateEnemy, null, OnPutBackInPool, defaultCapacity: 50);
        database = GameObject.Find("Database").GetComponent<LoadDatabase>();
        scaler = database.scaler;
        floorCounter = GameObject.Find("Floor Counter").GetComponent<FloorTextOverlay>().floorCounter;
        numberOfEnemiesToSpawn += 2;
        dungeonGenerator.OnFinishBuilding += StartCoroutine;

    }

    private void OnPutBackInPool(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    private GameObject CreateEnemy()
    {
        int spawnIndex = 0;

        if(enemySpawnMethod == SpawnMethod.RoundRobin)
        {
            
        }
        else if(enemySpawnMethod == SpawnMethod.Random)
        {
            spawnIndex = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        }
        var enemy = Instantiate(enemyPrefabs[spawnIndex],transform);
        enemy.GetComponent<EnemyManager>().EnemyScaler(scaler, floorCounter);
        return enemy;
    }

    private void StartCoroutine()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        SpawnedEnemies = 0;
        EnemiesAlive =  0;
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);
        
        while(SpawnedEnemies < numberOfEnemiesToSpawn) 
        {
            SpawnEnemy();
           

            yield return wait;
            SpawnedEnemies++;

        }


        if(ContinousSpawning)
        {
            StartCoroutine(SpawnEnemies());
            Debug.Log("enemy spawned");
        }
    }


    private void SpawnEnemy()
    {
      
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            int VertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);
            Vector3 offset = new Vector3(rndNumber(), 0f, rndNumber());
            NavMeshHit Hit;
          
            float yRotation = UnityEngine.Random.Range(0, 4) * 90f;
        if (NavMesh.SamplePosition(triangulation.vertices[VertexIndex] + offset, out Hit, 2f, 11))
            {


                GameObject enemy = _enemyPool.Get();
                if (enemy == null) { return; }
                enemy.transform.Rotate(0, yRotation, 0);
                NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
                enemyAgent.Warp(Hit.position);
                enemy.GetComponent<EnemyAI>().SetSpawnPosition(Hit.position);
                enemyAgent.enabled = true;
                enemy.GetComponent<EnemyManager>().SetPool(_enemyPool);
                enemy.GetComponent<EnemyManager>().OnDie += HandleEnemyDeath;
                EnemiesAlive++;

        }
            else
            {
                //Debug.Log("Unable to place enemy on NavMesh");
            }
        
     
    }


    private void HandleEnemyDeath()
    {
        EnemiesAlive--;
        if(EnemiesAlive == 0 && SpawnedEnemies == numberOfEnemiesToSpawn)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
    float rndNumber()
    {
        return UnityEngine.Random.Range(0f, 10f);
    }
    private void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
   
}
